using CodingPatterns.ApplicationLayer.ServiceResultPattern._Enums;
using CodingPatterns.InfrastructureLayer.IntegrationEvents.GooglePubSub._Attributes;
using Infrastructure.API;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.AuctionOperations.API.EventListeners;

[ApiController]
[Route("api/[controller]")]
[SwaggerDoc("UserAuthWebhooks")]
[ApiVersion("1.0")]
public class UserAuthSubscriptions : IntegrationEventListeners
{
    private const string UserService = "Auth-service"; 
    private readonly IIntegrationEventHandler _integrationEventHandler;
    
    public UserAuthSubscriptions(IIntegrationEventHandler integrationEventHandler) : base(integrationEventHandler)
    {
        _integrationEventHandler = integrationEventHandler;
    }
    
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