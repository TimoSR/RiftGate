using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots.Abstract;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.AggregateRoots.Events;

namespace API.Features.AuctionListing.Domain.AggregateRoots;

public class TraditionalAuction : Auction
{
    public TraditionalAuction(string sellerId, Item item, AuctionLength auctionLength) : base(sellerId, item, auctionLength)
    {
    }

    public override void PlaceBid(Bid bid)
    {
        if (DateTime.UtcNow < StartTime || DateTime.UtcNow > EndTime || IsCompleted)
            throw new InvalidOperationException("The auction is not active.");

        var highestBid = GetCurrentHighestBid();
 
        if (highestBid != null && bid.BidAmount.Value <= highestBid.BidAmount.Value)
            throw new InvalidOperationException("Bid amount must be higher than the current highest bid.");
        
        _bids.Add(bid);

        AddDomainEvent(new NewBidPlacedDomainEvent(bid.Id, bid.BidAmount.Value));
    }
}