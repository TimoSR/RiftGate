using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.AggregateRoots.Events;
using CodingPatterns.DomainLayer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Features.AuctionListing.Domain.AggregateRoots;

public abstract class Auction : Entity, IAggregateRoot
{
    [BsonId] [BsonRepresentation(BsonType.ObjectId)] public override string Id { get; }
    
    private readonly ITimeService _timeService;
    public DateTime StartTime { get; private set; }
    public DateTime EstimatedEndTime;
    protected readonly List<Bid> Bids = new();
    private readonly AuctionLength _auctionLength;
    private readonly Item _item;
    private readonly string _sellerId;
    public bool IsCompleted { get; private set; }

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
        IsCompleted = false;
    }

    public void StartAuction()
    {
        StartTime = _timeService.GetCurrentTime();
        EstimatedEndTime = StartTime.AddHours(_auctionLength.Value);
        AddDomainEvent(new AuctionStartedEvent(Id, StartTime));
    }

    public void CompleteAuction()
    {
        IsCompleted = true;
        var completionTime = _timeService.GetCurrentTime();
        AddDomainEvent(new AuctionCompletedEvent(Id, completionTime));
    }

    public abstract void PlaceBid(Bid bid);

    protected Bid? GetCurrentHighestBid()
    {
        return Bids.MaxBy(b => b.BidAmount.Value);
    }
}