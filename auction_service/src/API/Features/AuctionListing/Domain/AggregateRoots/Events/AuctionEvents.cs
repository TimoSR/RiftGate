using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.Domain.AggregateRoots.Events;

public record AuctionCompletedEvent(string AuctionId, DateTime CompletionTime) : IDomainEvent
{
    public string Message => 
        $"Auction {AuctionId} completed at {CompletionTime:yyyy-MM-dd HH:mm:ss} (UTC).";
}

public readonly record struct AuctionStartedEvent(string AuctionId, DateTime StartTime) : IDomainEvent
{
    public string Message => 
        $"Auction {AuctionId} started at {StartTime:yyyy-MM-dd HH:mm:ss} (UTC).";
}

public readonly record struct BidPlacedEvent(string AuctionId, Bid Bid) : IDomainEvent
{
    public string Message => 
        $"New bid placed on auction {AuctionId}. " +
        $"Timestamp: {Bid.TimeStamp:yyyy-MM-dd HH:mm:ss} (UTC). " +
        $"Bid Amount: {Bid.BidAmount.Value:C}. " +
        $"Bidder: {Bid.BidderId}.";
}

public readonly record struct AuctionSoldEvent(string AuctionId, Bid WinningBid, DateTime SellingTime) : IDomainEvent
{
    public string Message => 
        $"Auction '{AuctionId}' was successfully sold to bidder '{WinningBid.BidderId}' " +
        $"with a winning bid of {WinningBid.BidAmount.Value:C} at {SellingTime:yyyy-MM-dd HH:mm:ss} (UTC).";
}

public readonly record struct AuctionExpiredEvent(string AuctionId, DateTime ExpiredTime) : IDomainEvent
{
    public string Message => 
        $"Auction '{AuctionId}' has expired without a winning bid at {ExpiredTime:yyyy-MM-dd HH:mm:ss} (UTC).";
}

