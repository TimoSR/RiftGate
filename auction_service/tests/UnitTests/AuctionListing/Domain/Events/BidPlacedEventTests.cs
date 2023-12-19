using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using API.Features.AuctionListing.Domain.AggregateRoots.Events;
using Moq;

namespace UnitTests.AuctionListing.Domain.Events;

public class BidPlacedEventTests
{

    private readonly Mock<ITimeService> _mockTimeService;
    private readonly DateTime _fixedDateTime;
    
    public BidPlacedEventTests()
    {
        _fixedDateTime = new DateTime(2023, 1, 1);
        _mockTimeService = new Mock<ITimeService>();
        _mockTimeService.Setup(service => service.GetCurrentTime()).Returns(_fixedDateTime);
    }
    
    [Fact]
    public void ShouldCorrectlySetProperties()
    {
        string auctionId = "Auction123";
        var bidAmount = new Price(100);
        var eventObj = new BidPlacedEvent(auctionId, bidAmount);

        Assert.Equal(auctionId, eventObj.AuctionId);
        Assert.Equal(bidAmount, eventObj.Amount);
        Assert.Equal($"New bid placed on auction {auctionId}. Bid Amount: {bidAmount.Value}", eventObj.Message);
    }
}