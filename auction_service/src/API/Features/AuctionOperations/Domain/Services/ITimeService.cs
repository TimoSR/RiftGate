using CodingPatterns.DomainLayer;

namespace API.Features.AuctionOperations.Domain.Services;

public interface ITimeService : IDomainService
{
    DateTime GetCurrentTime();
}