using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;

public class Bid : Entity
{
    public override string Id { get; }
    public Price BidAmount { get; }
    public string BidderId { get; }
    public DateTime TimeStamp { get; }

    public Bid(string bidderId, Price bidAmount)
    {
        Validate(bidderId);
        BidderId = bidderId;
        BidAmount = bidAmount;
        TimeStamp = DateTime.UtcNow;
    }

    private void Validate(string bidderId)
    {
        if (string.IsNullOrWhiteSpace(bidderId)) 
            throw new ArgumentNullException(nameof(bidderId), "Bidder ID cannot be null or whitespace.");
    }
}