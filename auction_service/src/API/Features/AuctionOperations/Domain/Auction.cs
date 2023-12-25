using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.Events;
using API.Features.AuctionOperations.Domain.Services;
using API.Features.AuctionOperations.Domain.ValueObjects;
using CodingPatterns.DomainLayer;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Features.AuctionOperations.Domain;

// Private Set is needed in Entities and AggregateRoots so MongoDB Driver can map the data.

[BsonDiscriminator("auction", RootClass = true)]
[BsonKnownTypes(typeof(BuyoutAuction), typeof(TraditionalAuction))]
public abstract class Auction : Entity, IAggregateRoot
{   
    [BsonElement("startTime")]
    public DateTime StartTime { get; private set; }

    [BsonElement("estimatedEndTime")]
    public DateTime EstimatedEndTime { get; private set; }
    
    [BsonElement("buyoutAmount")]
    public Price? BuyoutAmount { get; private set; }

    [BsonElement("bids")]
    public List<Bid> Bids { get; private set; } = new();

    [BsonElement("auctionLength")]
    public AuctionLength AuctionLengthHours { get; private set; }

    [BsonElement("item")]
    public Item Item { get; private set; }

    [BsonElement("sellerId")]
    public string SellerId { get; private set; }

    [BsonElement("isActive")]
    public bool IsActive { get; private set; }

    protected  Auction(
        string sellerId, 
        Item item, 
        AuctionLength auctionLength,
        Price? buyout)
    {
        SellerId = sellerId ?? throw new ArgumentNullException(nameof(sellerId));
        AuctionLengthHours = auctionLength ?? throw new ArgumentNullException(nameof(auctionLength));
        Item = item ?? throw new ArgumentNullException();
        BuyoutAmount = buyout;
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
        EstimatedEndTime = StartTime.AddHours(AuctionLengthHours.Value);
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