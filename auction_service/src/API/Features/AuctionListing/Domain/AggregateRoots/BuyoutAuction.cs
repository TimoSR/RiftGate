using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.AggregateRoots.Events;

namespace API.Features.AuctionListing.Domain.AggregateRoots;

public class BuyoutAuction : Auction
{
    private Price Buyout { get; set; }
    
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
            throw new InvalidOperationException("The auction is not active.");

        var highestBid = GetCurrentHighestBid();
 
        if (highestBid != null && bid.BidAmount.Value <= highestBid.BidAmount.Value)
            throw new InvalidOperationException("Bid amount must be higher than the current highest bid.");

        if (highestBid != null && bid.BidAmount.Value >= Buyout.Value)
            throw new InvalidOperationException("Bid is higher than buyout price!");

        Bids.Add(bid);

        AddDomainEvent(new BidPlacedEvent(Id, bid));
    }
}