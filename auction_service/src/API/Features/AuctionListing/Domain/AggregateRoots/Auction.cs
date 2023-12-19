using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.AggregateRoots.Events;
using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.Domain.AggregateRoots;

public abstract class Auction : Entity, IAggregateRoot
{
    private readonly ITimeService _timeService;
    public DateTime StartTime { get; private set; }
    public DateTime EstimatedEndTime { get; private set; }
    protected readonly List<Bid> Bids = new();
    private readonly AuctionLength _auctionLength;
    private readonly Item _item;
    private readonly string _sellerId;
    public bool IsActive { get; private set; }

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