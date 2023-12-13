using _SharedKernel.Patterns.ResultPattern;
using _SharedKernel.Patterns.ResultPattern._Enums;
using Application.AppServices._Interfaces;
using Application.DTO.UserManagement;
using Domain._Shared.Events.Subscribed.UserAuthentication;
using Domain._Shared.Events.Topics.UserManagement;
using Domain.UserManagement.Entities;
using Domain.UserManagement.Enums;
using Domain.UserManagement.Messages;
using Domain.UserManagement.Repositories;
using Domain.UserManagement.Services._Interfaces;
using Infrastructure.Persistence._Interfaces;
using Newtonsoft.Json;

namespace Application.AppServices.V1;

public class UserManagerService : IUserService
{
    private readonly IUserValidationService _userValidationService;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<UserManagerService> _logger;
    private readonly IEventHandler _eventHandler;
    private readonly ICacheManager _cacheManager;

    public UserManagerService(
        IUserValidationService userValidationService,
        IUserRepository userRepository,
        ILogger<UserManagerService> logger,
        IEventHandler eventHandler,
        ICacheManager cacheManager
    )
    {
        _userValidationService = userValidationService;
        _userRepository = userRepository;
        _logger = logger;
        _eventHandler = eventHandler;
        _cacheManager = cacheManager;
    }
    
    public async Task<ServiceResult> RegisterAsync(UserRegisterDto userDto)
    {
        _logger.LogInformation("Starting user registration process for Email: {Email}", userDto.Email);
        
        try
        {
            var validationResult = _userValidationService.ValidateNewUser(userDto.Email, userDto.Password);
            if (!validationResult.IsSuccess)
            {
                _logger.LogWarning("User registration validation failed for Email: {Email} - Reasons: {Reasons}", userDto.Email, string.Join(", ", validationResult.Messages));
                return ServiceResult.Failure(validationResult.Messages, ServiceErrorType.BadRequest);
            }

            var newUser = new User
            {
                Email = userDto.Email,
                UserName = userDto.UserName
            };
            
            bool registrationSuccess = await _userRepository.CreateUserIfNotRegisteredAsync(newUser);
            if (!registrationSuccess)
            {
                _logger.LogWarning("User registration failed for Email: {Email} - Email already exists", userDto.Email);
                return ServiceResult.Failure(UserRegMsg.EmailAlreadyExists, ServiceErrorType.BadRequest);
            }

            _logger.LogInformation("User registration completed successfully for Email: {Email}", newUser.Email);

            var userRegInit = new UserRegInitEvent()
            {
                Email = userDto.Email,
                Password = userDto.Password
            };

            await _eventHandler.PublishProtobufEventAsync(userRegInit);
            return ServiceResult.Success("User successfully registered.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred during the user registration process for Email: {Email}", userDto.Email);
            return ServiceResult.Failure( "An unexpected error occurred.");
        }
    }

    public async Task<ServiceResult<UserDto>> GetUserByEmailAsync(string email)
    {
        // First, try to get the user from cache
        var cacheKey = $"user_{email}";
        var cachedUser = await _cacheManager.GetValueAsync(cacheKey);
        if (cachedUser.IsSuccess && cachedUser.Value != null)
        {
            return ServiceResult<UserDto>.Success(JsonConvert.DeserializeObject<UserDto>(cachedUser.Value));
        }

        // If not in cache, retrieve from the database
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null)
        {
            _logger.LogWarning("User with Email: {email} not found.", email);
            return ServiceResult<UserDto>.Failure("User not found.", ServiceErrorType.NotFound);
        }

        // Cache the retrieved user
        var userDto = new UserDto() { Email = user.Email, UserName = user.UserName};
        await _cacheManager.SetValueAsync(cacheKey, JsonConvert.SerializeObject(userDto), TimeSpan.FromSeconds(4)); // Set an appropriate expiration

        return ServiceResult<UserDto>.Success(userDto);
    }
    
    public async Task<ServiceResult> DeleteUserByEmailAsync(string email)
    {
        _logger.LogInformation("Attempting to delete user with Email: {Email}", email);

        try
        {
            // Deleting the user
            bool deleteSuccess = await _userRepository.DeleteUserByEmailAsync(email);
            if (!deleteSuccess)
            {
                _logger.LogWarning("Deletion failed for user with Email: {Email}", email);
                return ServiceResult.Failure("User not found or deletion failed.", ServiceErrorType.NotFound);
            }

            // Invalidate the cache
            var cacheKey = $"user_{email}";
            await _cacheManager.RemoveValueAsync(cacheKey);

            var userDeletionInitEvent = new UserDeletionInitEvent { Email = email };
            
            await _eventHandler.PublishProtobufEventAsync(userDeletionInitEvent);
            
            _logger.LogInformation("Successfully deleted user with Email: {email}", email);
            return ServiceResult.Success("User deleted successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the deletion process for user with Email: {email}", email);
            return ServiceResult.Failure("An unexpected error occurred during deletion.");
        }
    }

    
    public async Task<ServiceResult> UpdateUserStatusByEmailAsync(UserAuthDetailsSetSuccessEvent @event, UserStatus status)
    {
        _logger.LogInformation("Attempting to update status of user with email: {Email}", @event.Email);

        try
        {
            bool updateSuccess = await _userRepository.UpdateUserStatusByEmailAsync(@event.Email, status);

            if (!updateSuccess)
            {
                _logger.LogWarning("Update of user status failed for email: {Email}", @event.Email);
                return ServiceResult.Failure("User not found or update failed.", ServiceErrorType.NotFound);
            }
            
            // Update cache if necessary
            var user = await _userRepository.GetUserByEmailAsync(@event.Email);
            if (user != null)
            {
                var cacheKey = $"user_{user.Id}";
                var userDto = new UserDto() { Email = user.Email };
                await _cacheManager.SetValueAsync(cacheKey, JsonConvert.SerializeObject(userDto), TimeSpan.FromMinutes(30));
            }

            _logger.LogInformation("Status of user with email: {Email} updated successfully to {Status}", @event.Email, status);
            return ServiceResult.Success("User status updated successfully.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the update process for user status with email: {Email}", @event.Email);
            return ServiceResult.Failure("An unexpected error occurred during update.");
        }
    }
    
    // In UserManagerService class
    public async Task<ServiceResult> RollBackUserAsync(UserAuthDetailsSetFailedEvent @event)
    {
        _logger.LogInformation("Attempting to rollback user with email: {Email}", @event.Email);

        try
        {
            bool rollbackSuccess = await _userRepository.DeleteUserByEmailAsync(@event.Email);

            if (!rollbackSuccess)
            {
                _logger.LogWarning("Rollback failed for user with email: {Email}", @event.Email);
                return ServiceResult.Failure("Rollback failed.");
            }

            _logger.LogInformation("Rollback was successful for user with email: {Email}", @event.Email);
            return ServiceResult.Success("Rollback successful.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred during the rollback process for user with email: {Email}", @event.Email);
            return ServiceResult.Failure("An unexpected error occurred during rollback.");
        }
    }
}