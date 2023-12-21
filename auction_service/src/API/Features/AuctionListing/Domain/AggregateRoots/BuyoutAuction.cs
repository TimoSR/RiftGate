using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.AggregateRoots.Events;

namespace API.Features.AuctionListing.Domain.AggregateRoots;

public class BuyoutAuction : Auction
{
    private Price Buyout { get; }
    
    public BuyoutAuction(
        string sellerId, 
        Item item, 
        AuctionLength auctionLength, 
        Price buyout,
        ITimeService timeService) 
        : base(sellerId, item, auctionLength, timeService)
    {
        Buyout = buyout ?? throw new ArgumentNullException(nameof(buyout));
    }

    public override void PlaceBid(Bid bid)
    {
        if (!IsActive)
            throw new InvalidOperationException("Attempted to place a bid on an inactive auction.");

        var highestBid = GetCurrentHighestBid();

        if (highestBid != null && bid.BidAmount.Value <= highestBid.BidAmount.Value)
        {
            throw new InvalidOperationException($"Bid amount of {bid.BidAmount.Value} must be higher than the current highest bid of {highestBid.BidAmount.Value}.");
        }

        if (bid.BidAmount.Value >= Buyout.Value)
        {
            throw new InvalidOperationException($"Bid of {bid.BidAmount.Value} exceeds or equals the buyout price of {Buyout.Value}, which is not allowed.");
        }

        Bids.Add(bid);
        AddDomainEvent(new BidPlacedEvent(Id, bid.BidAmount));
    }
    
    private void HandleBuyoutCondition(Bid bid)
    {
        if (bid.BidAmount.Value >= Buyout.Value)
        {
            IsActive = false;
            var completionTime = _timeService.GetCurrentTime();
            AddDomainEvent(new AuctionCompletedEvent(Id, completionTime));
        }
    }
}