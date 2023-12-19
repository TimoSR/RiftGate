namespace API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;

public interface ITimeService
{
    DateTime GetCurrentTime();
}