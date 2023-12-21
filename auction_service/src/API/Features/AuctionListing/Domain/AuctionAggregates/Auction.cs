using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AuctionAggregates.DomainService;
using API.Features.AuctionListing.Domain.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.AuctionAggregates.Events;
using API.Features.AuctionListing.Domain.AuctionAggregates.ValueObjects;
using CodingPatterns.DomainLayer;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Features.AuctionListing.Domain.AuctionAggregates;

[BsonDiscriminator("auction", RootClass = true)]
[BsonKnownTypes(typeof(BuyoutAuction), typeof(TraditionalAuction))]
public abstract class Auction : Entity, IAggregateRoot
{   
    public DateTime StartTime { get; private set; }
    public DateTime EstimatedEndTime { get; private set; }
    public List<Bid> Bids { get; } = new();
    public AuctionLength AuctionLength { get; }
    public Item Item { get; private set; }
    public string SellerId { get; private set; }
    public bool IsActive { get; private set; }

    protected  Auction(
        string sellerId, 
        Item item, 
        AuctionLength auctionLength)
    {
        SellerId = sellerId ?? throw new ArgumentNullException(nameof(sellerId));
        AuctionLength = auctionLength ?? throw new ArgumentNullException(nameof(auctionLength));
        Item = item ?? throw new ArgumentNullException();
    }
    
    // Virtual Methods
    public virtual void PlaceBid(Bid bid)
    {
        ValidateBid(bid);
        
        Bids.Add(bid);

        AddDomainEvent(new BidPlacedEvent(Id, bid));  
    }

    protected virtual void ValidateBid(Bid bid)
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
    }
    
    // Public
    
    public void StartAuction(ITimeService timeService)
    {
        if(timeService == null)
            throw new ArgumentNullException(nameof(timeService));

        StartTime = timeService.GetCurrentTime();
        EstimatedEndTime = StartTime.AddHours(AuctionLength.Value);
        IsActive = true;
        AddDomainEvent(new AuctionStartedEvent(Id, StartTime));
    }
    
    public void CheckAndCompleteAuction(ITimeService timeService)
    {
        if(timeService == null)
            throw new ArgumentNullException(nameof(timeService));

        var currentTime = timeService.GetCurrentTime();
        if (IsAuctionExpired(currentTime))
        {
            CompleteAuction(currentTime);
        }
    }
    
    // Protected
    
    private protected void CompleteAuction(DateTime completionTime)
    {
        if (!IsActive)
            throw new InvalidOperationException("Auction is not active.");

        IsActive = false;
        var highestBid = GetCurrentHighestBid();

        if (highestBid != null)
        {
            // Auction is completed with a sale.
            AddDomainEvent(new AuctionSoldEvent(Id, highestBid, completionTime));
        }
        else
        {
            // Auction is expired without any bids.
            AddDomainEvent(new AuctionExpiredEvent(Id, completionTime));
        }
    }
    
    public Bid? GetCurrentHighestBid()
    {
        return Bids.MaxBy(b => b.BidAmount.Value);
    }
    
    // Private

    private bool IsAuctionExpired(DateTime currentTime)
    {
        return EstimatedEndTime <= currentTime;
    }
}