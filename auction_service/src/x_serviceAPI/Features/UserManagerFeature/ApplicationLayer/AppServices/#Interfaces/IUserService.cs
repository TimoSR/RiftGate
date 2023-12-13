using Application.DTO.UserManagement;
using CodingPatterns.ApplicationLayer.ApplicationServices;
using CodingPatterns.ApplicationLayer.ServiceResultPattern;
using Domain.UserManagement.Enums;
using x_serviceAPI.Features.UserManagerFeature.ApplicationLayer.Events.Subscribed.UserAuthentication;

namespace x_serviceAPI.Features.UserManagerFeature.ApplicationLayer.AppServices._Interfaces;

public interface IUserService : IAppService
{

    Task<ServiceResult> RollBackUserAsync(UserAuthDetailsSetFailedEvent @event);
    Task<ServiceResult> UpdateUserStatusByEmailAsync(UserAuthDetailsSetSuccessEvent @event, UserStatus status);
    Task<ServiceResult> RegisterAsync(UserRegisterDto userDto);
    Task<ServiceResult<UserDto>> GetUserByEmailAsync(string email);
    Task<ServiceResult> DeleteUserByEmailAsync(string email);
}