using API.Features.AuctionOperations.API.EventListeners.Subscriptions;
using CodingPatterns.InfrastructureLayer.IntegrationEvents.GooglePubSub._Attributes;
using Infrastructure.API;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Infrastructure.UtilityServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Features.AuctionOperations.API.EventListeners;

[ApiController]
[Route("api/[controller]")]
[SwaggerDoc("UserAuctionSubscriptions")]
[ApiVersion("1.0")]
[Authorize]
public class UserAuctionSubscriptions : IntegrationEventListeners
{
    private const string UserService = "auction-service";
    private ILogger<UserAuctionSubscriptions> _logger;
    
    public UserAuctionSubscriptions(
        IIntegrationEventHandler integrationEventHandler,
        IProtobufSerializer protobufSerializer,
        ILogger<UserAuctionSubscriptions> logger) : base(integrationEventHandler, protobufSerializer)
    {
        _logger = logger;
    }
    
    [AllowAnonymous]
    [HttpPost("HandleAuctionStartedEvent")]
    [EventSubscription($"{UserService}-AuctionStartedTopic")]
    public async Task<IActionResult> HandleAuctionStartedEvent()
    {
        var data = await OnEvent<AuctionStartedIntegrationEvent>();
        _logger.LogInformation(
            $"Testing Serialization Data is Programmable. " +
            $"AuctionId: {data?.AuctionId} " +
            $"StartTime: {data?.StartTime} (UTC)");
        return Ok();
    }
}