using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.AggregateRoots.Events;
using CodingPatterns.DomainLayer;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Features.AuctionListing.Domain.AggregateRoots;

[BsonDiscriminator("auction", RootClass = true)]
[BsonKnownTypes(typeof(BuyoutAuction), typeof(TraditionalAuction))]
public abstract class Auction : Entity, IAggregateRoot
{
    protected readonly ITimeService _timeService;
    public DateTime StartTime { get; private set; }
    public DateTime EstimatedEndTime { get; private set; }
    protected readonly List<Bid> Bids = new();
    private readonly AuctionLength _auctionLength;
    public Item _item { get; private set; }
    public string _sellerId { get; private set; }
    protected bool IsActive { get;  set; }

    protected Auction(
        string sellerId, 
        Item item, 
        AuctionLength auctionLength,
        ITimeService timeService)
    {
        _sellerId = sellerId ?? throw new ArgumentNullException(nameof(sellerId));
        _auctionLength = auctionLength ?? throw new ArgumentNullException(nameof(auctionLength));
        _timeService = timeService ?? throw new ArgumentNullException();
        _item = item ?? throw new ArgumentNullException();
    }

    public void StartAuction()
    {
        StartTime = _timeService.GetCurrentTime();
        EstimatedEndTime = StartTime.AddHours(_auctionLength.Value);
        IsActive = true;
        AddDomainEvent(new AuctionStartedEvent(Id, StartTime));
    }

    public bool HaveAuctionExpired()
    {
        var currentTime = _timeService.GetCurrentTime();
        return StartTime <= currentTime && currentTime <= EstimatedEndTime && !IsActive;
    }

    public void CompleteAuction()
    {
        IsActive = false;
        var completionTime = _timeService.GetCurrentTime();
        AddDomainEvent(new AuctionCompletedEvent(Id, completionTime));
    }

    public abstract void PlaceBid(Bid bid);

    protected Bid? GetCurrentHighestBid()
    {
        return Bids.MaxBy(b => b.BidAmount.Value);
    }
}