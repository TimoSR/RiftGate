using _SharedKernel.Patterns.RegistrationHooks.Services._Interfaces;
using _SharedKernel.Patterns.ResultPattern;
using Application.DTO.UserManagement;
using Domain._Shared.Events.Subscribed.UserAuthentication;
using Domain.UserManagement.Enums;

namespace Application.AppServices._Interfaces;

public interface IUserService : IAppService
{
    Task<ServiceResult> UpdateUserStatusByEmailAsync(UserAuthDetailsSetSuccessEvent @event, UserStatus status);
    Task<ServiceResult> RollBackUserAsync(UserAuthDetailsSetFailedEvent @event);
    Task<ServiceResult> RegisterAsync(UserRegisterDto userDto);
    Task<ServiceResult<UserDto>> GetUserByEmailAsync(string email);
    Task<ServiceResult> DeleteUserByEmailAsync(string email);
}