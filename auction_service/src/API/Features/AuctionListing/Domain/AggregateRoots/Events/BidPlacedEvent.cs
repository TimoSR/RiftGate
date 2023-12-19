using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.Domain.AggregateRoots.Events;

public readonly record struct BidPlacedEvent(string AuctionId, Price Amount) : IDomainEvent
{
    public string Message => $"New bid placed on auction {AuctionId}. Bid Amount: {Amount.Value}";
}