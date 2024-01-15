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
    
    public UserAuctionSubscriptions(
        IIntegrationEventHandler integrationEventHandler,
        IProtobufSerializer protobufSerializer
    ) : base(integrationEventHandler, protobufSerializer)
    {

    }
    
    [AllowAnonymous]
    [HttpPost("HandleAuctionStartedEvent")]
    [EventSubscription($"{UserService}-AuctionStartedTopic")]
    public async Task<IActionResult> HandleAuctionStartedEvent()
    {
        Console.WriteLine("Something");
        var data = await OnEvent<AuctionStartedIntegrationEvent>();
        Console.Write($"Testing Serialization Data is Programmable. " +
                      $"AuctionId: {data?.AuctionId} " +
                      $"StartTime: {data?.StartTime}");
        return Ok();
    }
}