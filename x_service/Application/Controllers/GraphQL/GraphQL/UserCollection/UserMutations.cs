using _SharedKernel.Patterns.ResultPattern;
using Application.AppServices._Interfaces;
using Application.Controllers.GraphQL.GraphQL._Interfaces;
using Application.DTO.UserManagement;
using HotChocolate.Subscriptions;

namespace Application.Controllers.GraphQL.GraphQL.UserCollection;

public class UserMutations : IMutation
{
    private readonly IUserService _userService;
    private readonly ITopicEventSender _sender;
    
    public UserMutations(IUserService userService, ITopicEventSender sender)
    {
        _userService = userService;
        _sender = sender;
    }
    
    public async Task<ServiceResult> RegisterUser(UserRegisterDto newUserDto)
    {
        var result = await _userService.RegisterAsync(newUserDto);

        if (!result.IsSuccess) return result;
        
        await _sender.SendAsync(nameof(UserSubscriptions.OnUserRegistered), result);

        return result;
    }
}