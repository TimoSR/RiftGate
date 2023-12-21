using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.AggregateRoots.Events;

namespace API.Features.AuctionListing.Domain.AggregateRoots;

public class TraditionalAuction : Auction
{
    public TraditionalAuction(
        string sellerId, 
        Item item, 
        AuctionLength auctionLength) 
        : base(sellerId, item, auctionLength)
    {
    }

    public override void PlaceBid(Bid bid)
    {
        if (!IsActive)
            throw new InvalidOperationException("Attempted to place a bid on an inactive auction.");

        var highestBid = GetCurrentHighestBid();
 
        if (highestBid != null && bid.BidAmount.Value <= highestBid.BidAmount.Value)
            throw new InvalidOperationException($"Bid amount of {bid.BidAmount.Value} must be higher than the current highest bid of {highestBid.BidAmount.Value}.");
        
        Bids.Add(bid);

        AddDomainEvent(new BidPlacedEvent(Id, bid));
    }
}