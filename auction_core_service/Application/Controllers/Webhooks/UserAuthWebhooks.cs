using _SharedKernel.Patterns.RegistrationHooks.Events._Attributes;
using _SharedKernel.Patterns.ResultPattern._Enums;
using Application.AppServices._Interfaces;
using Application.Controllers.Webhooks._Abstract;
using Domain._Shared.Events.Subscribed.UserAuthentication;
using Domain._Shared.Events.Topics.UserManagement;
using Domain.UserManagement.Enums;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.Webhooks;

[ApiController]
[Route("api/[controller]")]
[SwaggerDoc("UserAuthWebhooks")]
[ApiVersion("1.0")]
[Authorize]
public class UserAuthWebhooks : BaseWebhookController
{
    private const string UserService = "Auth-service"; 
    private readonly IUserService _userService;
    private readonly IEventHandler _eventHandler;
    
    public UserAuthWebhooks(
        IUserService userService,
        IEventHandler eventHandler
        ) : base(eventHandler)
    {
        _userService = userService;
        _eventHandler = eventHandler;
    }
    
    [AllowAnonymous]
    [HttpPost("HandleUserAuthDetailsSetSuccessEvent")]
    [EventSubscription($"{UserService}-UserAuthDetailsSetSuccessTopic")]
    public async Task<IActionResult> HandleUserAuthDetailsSetSuccessEvent()
    {
        var data = await OnEvent<UserAuthDetailsSetSuccessEvent>();
        var result = await _userService.UpdateUserStatusByEmailAsync(data, UserStatus.Active);

        if (result.IsSuccess)
        {
            await _eventHandler.PublishProtobufEventAsync(new UserRegCompletedEvent{Email = data.Email});
            return Ok(new { result.Messages });
        }
        
        return result.ErrorType switch
        {
            ServiceErrorType.BadRequest => BadRequest(new { Message = result.Messages }),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    [AllowAnonymous]
    [HttpPost("HandleUserAuthDetailsSetFailedEvent")]
    [EventSubscription($"{UserService}-UserAuthDetailsSetFailedTopic")]
    public async Task<IActionResult> HandleUserAuthDetailsSetFailedEvent()
    {
        var data = await OnEvent<UserAuthDetailsSetFailedEvent>();
        var result = await _userService.RollBackUserAsync(data);

        if (result.IsSuccess)
        {
            return Ok(new { result.Messages });
        }

        return result.ErrorType switch
        {
            ServiceErrorType.BadRequest => BadRequest(new { Message = result.Messages }),
            _ => throw new ArgumentOutOfRangeException()
        };
    }
    
    [AllowAnonymous]
    [HttpPost("HandleUserDeletionSuccessEvent")]
    [EventSubscription($"{UserService}-UserDeletionSuccessTopic")]
    public async Task<IActionResult> HandleUserDeletionSuccessEvent()
    {
        var data = await OnEvent<UserDeletionSuccessEvent>();

        if (data != null)
        {
            await _eventHandler.PublishProtobufEventAsync(new UserDeletionCompleted { Email = data.Email });
            return Ok();
        }

        return BadRequest();
    }
}