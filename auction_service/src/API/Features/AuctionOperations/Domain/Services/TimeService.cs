using CodingPatterns.DomainLayer;

namespace API.Features.AuctionOperations.Domain.Services;

public class TimeService : ITimeService, IDomainService
{
    public DateTime GetCurrentTime()
    {
        return DateTime.UtcNow;
    }
}