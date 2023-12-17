using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using CodingPatterns.DomainLayer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Features.AuctionListing.Domain.AggregateRoots.Abstract;

public abstract class Auction : Entity, IAggregateRoot
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public override string Id { get; }
    public string SellerId { get; private set; }
    public Item Item { get; private set; }
    protected AuctionLength AuctionLength { get; private set; }
    protected DateTime StartTime { get; private set; }
    protected DateTime EndTime { get; private set; }
    protected bool IsCompleted { get; private set; }
    protected readonly List<Bid> _bids = new List<Bid>();

    protected Auction(string sellerId, Item item, AuctionLength auctionLength)
    {
        SellerId = sellerId ?? throw new ArgumentNullException(nameof(sellerId));
        Item = item ?? throw new ArgumentNullException(nameof(item));
        AuctionLength = auctionLength ?? throw new ArgumentNullException(nameof(auctionLength));
        IsCompleted = false;
    }

    public void StartAuction()
    {
        StartTime = DateTime.UtcNow;
        EndTime = StartTime.AddHours(AuctionLength.Value);
        // Optionally raise an event indicating the auction has started
    }

    protected void CompleteAuction()
    {
        IsCompleted = true;
        // Optionally raise an event indicating the auction has ended
    }

    public abstract void PlaceBid(Bid bid);

    protected Bid? GetCurrentHighestBid()
    {
        return _bids.MaxBy(b => b.BidAmount.Value);
    }
}