using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using API.Features.AuctionListing.Domain.AggregateRoots.Events;
using Moq;

namespace UnitTests.AuctionListing.Domain.Events;

public class AuctionStartedEventTests
{

    private readonly DateTime _fixedDateTime = new(2023, 1, 1);

    [Fact]
    public void ShouldCorrectlySetProperties()
    {
        string auctionId = "Auction123";
        DateTime startTime = _fixedDateTime;
        var eventObj = new AuctionStartedEvent(auctionId, startTime);

        Assert.Equal(auctionId, eventObj.AuctionId);
        Assert.Equal(startTime, eventObj.StartTime);
        Assert.Equal($"Auction {auctionId} started at {startTime}.", eventObj.Message);
    }
}