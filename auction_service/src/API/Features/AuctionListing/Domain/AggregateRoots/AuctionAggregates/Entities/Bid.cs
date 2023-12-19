using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;

public class Bid : Entity
{
    public Price BidAmount { get; }
    public string BidderId { get; }
    public DateTime TimeStamp { get; }

    public Bid(string bidderId, Price bidAmount, ITimeService timeService)
    {
        Validate(bidderId, bidAmount, timeService);
        BidderId = bidderId;
        BidAmount = bidAmount;
        TimeStamp = timeService.GetCurrentTime();
    }

    private static void Validate(string bidderId, Price bidAmount, ITimeService timeService)
    {
        if (string.IsNullOrWhiteSpace(bidderId)) 
            throw new ArgumentException("Bidder ID cannot be null or whitespace.", nameof(bidderId));
        
        if (bidAmount == null)
            throw new ArgumentNullException(nameof(bidAmount), "cannot be null.");

        if (timeService == null)
        {
            throw new ArgumentNullException(nameof(timeService), "cannot be null.");
        }
    }
}