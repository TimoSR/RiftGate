using API.Features.UserManagerFeature.ApplicationLayer.DTO.UserManagement;
using API.Features.UserManagerFeature.ApplicationLayer.Events.Subscribed.UserAuthentication;
using API.Features.UserManagerFeature.DomainLayer.Enums;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;

namespace API.Features.UserManagerFeature.ApplicationLayer.AppServices._Interfaces;

public interface IUserService : IAppService
{

    Task<ServiceResult> RollBackUserAsync(UserAuthDetailsSetFailedEvent @event);
    Task<ServiceResult> UpdateUserStatusByEmailAsync(UserAuthDetailsSetSuccessEvent @event, UserStatus status);
    Task<ServiceResult> RegisterAsync(UserRegisterDto userDto);
    Task<ServiceResult<UserDto>> GetUserByEmailAsync(string email);
    Task<ServiceResult> DeleteUserByEmailAsync(string email);
}