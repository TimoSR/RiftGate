using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.AggregateRoots.Events;
using CodingPatterns.DomainLayer;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Features.AuctionListing.Domain.AggregateRoots;

[BsonDiscriminator("auction", RootClass = true)]
[BsonKnownTypes(typeof(BuyoutAuction), typeof(TraditionalAuction))]
public abstract class Auction : Entity, IAggregateRoot
{   
    public DateTime StartTime { get; private set; }
    public DateTime EstimatedEndTime { get; private set; }
    public List<Bid> Bids { get; protected set; } = new();
    public AuctionLength AuctionLength { get; private set; }
    public Item Item { get; private set; }
    public string SellerId { get; private set; }
    public bool IsActive { get; private set; }

    protected Auction(
        string sellerId, 
        Item item, 
        AuctionLength auctionLength)
    {
        SellerId = sellerId ?? throw new ArgumentNullException(nameof(sellerId));
        AuctionLength = auctionLength ?? throw new ArgumentNullException(nameof(auctionLength));
        Item = item ?? throw new ArgumentNullException();
    }
    
    // Public
    
    public void StartAuction(DateTime startTime)
    {
        StartTime = startTime;
        EstimatedEndTime = startTime.AddHours(AuctionLength.Value);
        IsActive = true;
        AddDomainEvent(new AuctionStartedEvent(Id, StartTime));
    }
    
    public void CheckAndCompleteAuction(DateTime currentTime)
    {
        if (IsAuctionExpired(currentTime) && IsActive)
        {
            CompleteAuction(currentTime);
        }
    }
    
    public abstract void PlaceBid(Bid bid);
    
    // Protected
    
    protected void CompleteAuction(DateTime completionTime)
    {
        if (!IsActive)
            throw new InvalidOperationException("Auction is not active.");

        IsActive = false;
        AddDomainEvent(new AuctionCompletedEvent(Id, completionTime));
    }
    
    protected Bid? GetCurrentHighestBid()
    {
        return Bids.MaxBy(b => b.BidAmount.Value);
    }
    
    // Private

    private bool IsAuctionExpired(DateTime currentTime)
    {
        return EstimatedEndTime <= currentTime;
    }
}