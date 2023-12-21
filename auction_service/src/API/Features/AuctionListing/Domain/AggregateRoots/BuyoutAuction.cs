using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.AggregateRoots.Events;

namespace API.Features.AuctionListing.Domain.AggregateRoots;

public class BuyoutAuction : Auction
{
    public Price Buyout { get; }
    
    public BuyoutAuction(
        string sellerId, 
        Item item, 
        AuctionLength auctionLength, 
        Price buyout) 
        : base(sellerId, item, auctionLength)
    {
        Buyout = buyout ?? throw new ArgumentNullException(nameof(buyout));
    }
    
    // Public (Input Should be Validated)

    public override void PlaceBid(Bid bid)
    {
        ValidateBid(bid);
        
        AddDomainEvent(new BidPlacedEvent(Id, bid));
        
        HandleBuyoutCondition(bid);

        Bids.Add(bid);
    }
    
    // Private

    protected override void ValidateBid(Bid bid)
    {
        if (bid == null)
            throw new ArgumentNullException(nameof(bid), "Bid cannot be null.");
        
        if (!IsActive)
            throw new InvalidOperationException("Attempted to place a bid on an inactive auction.");
        
        var highestBid = GetCurrentHighestBid();

        if (highestBid != null && bid.BidAmount.Value <= highestBid.BidAmount.Value)
        {
            throw new InvalidOperationException($"Bid amount of {bid.BidAmount.Value} must be higher than the current highest bid of {highestBid.BidAmount.Value}.");
        }

        if (bid.BidAmount.Value > Buyout.Value)
        {
            throw new InvalidOperationException($"Bid of {bid.BidAmount.Value} exceeds or equals the buyout price of {Buyout.Value}, which is not allowed.");
        }
    }
    
    private void HandleBuyoutCondition(Bid bid)
    {
        if (bid.BidAmount.Value >= Buyout.Value)
        {
            CompleteAuction(bid.TimeStamp);
        }
    }
}