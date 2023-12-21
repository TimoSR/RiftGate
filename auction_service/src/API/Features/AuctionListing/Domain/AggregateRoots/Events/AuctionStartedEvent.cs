using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.Domain.AggregateRoots.Events;

public readonly record struct AuctionStartedEvent(string AuctionId, DateTime StartTime) : IDomainEvent
{
    public string Message => $"Auction {AuctionId} started at {StartTime}.";
}
