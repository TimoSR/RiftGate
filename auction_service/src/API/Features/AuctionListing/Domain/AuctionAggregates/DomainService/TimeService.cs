namespace API.Features.AuctionListing.Domain.AuctionAggregates.DomainService;

public class TimeService : ITimeService
{
    public DateTime GetCurrentTime()
    {
        return DateTime.UtcNow;
    }
}