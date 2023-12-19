using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.Domain.AggregateRoots.Events;

public readonly record struct AuctionCompletedEvent(string AuctionId, DateTime CompletionTime) : IDomainEvent
{
    public string Message => $"Auction {AuctionId} completed at {CompletionTime}.";
}