using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using CodingPatterns.ApplicationLayer.ServiceResultPattern._Enums;
using Domain.UserManagement.Enums;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using x_serviceAPI.Features._Common;
using x_serviceAPI.Features.UserManagerFeature.ApplicationLayer.AppServices._Interfaces;
using x_serviceAPI.Features.UserManagerFeature.ApplicationLayer.Events.Subscribed.UserAuthentication;
using x_serviceAPI.Features.UserManagerFeature.ApplicationLayer.Events.Topics.UserManagement;

namespace x_serviceAPI.Features.UserManagerFeature.API.V1.Webhooks;

[ApiController]
[Route("api/[controller]")]
[SwaggerDoc("UserAuthWebhooks")]
[ApiVersion("1.0")]
[Authorize]
public class UserAuthWebhooks : BaseWebhookController
{
    private const string UserService = "Auth-service"; 
    private readonly IUserService _userService;
    private readonly IIntegrationEventHandler _eventHandler;
    
    public UserAuthWebhooks(
        IUserService userService,
        IIntegrationEventHandler eventHandler
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