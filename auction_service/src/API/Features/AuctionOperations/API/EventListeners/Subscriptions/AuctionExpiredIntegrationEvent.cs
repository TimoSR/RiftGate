using CodingPatterns.InfrastructureLayer.IntegrationEvents;

namespace API.Features.AuctionOperations.API.EventListeners.Subscriptions;

public record AuctionExpiredIntegrationEvent : ISubscribeEvent
{
    
}