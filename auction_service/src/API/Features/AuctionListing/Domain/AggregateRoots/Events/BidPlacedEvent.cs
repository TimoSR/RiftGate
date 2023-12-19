using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.Domain.AggregateRoots.Events;

public readonly record struct BidPlacedEvent(string AuctionId, Bid Bid) : IDomainEvent
{
    public string Message => $"New bid placed on auction {AuctionId}. Bid Amount: {Bid.BidAmount.Value}";
}