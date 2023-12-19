namespace API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;

public class TimeService : ITimeService
{
    public DateTime GetCurrentTime()
    {
        return DateTime.UtcNow;
    }
}