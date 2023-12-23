namespace API.Features.AuctionManagement.Domain.Services;

public class TimeService : ITimeService
{
    public DateTime GetCurrentTime()
    {
        return DateTime.UtcNow;
    }
}