using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
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
        var bid = new Bid("Bidder123", bidAmount, _mockTimeService.Object);
        var eventObj = new BidPlacedEvent(auctionId, bid);

        Assert.Equal(auctionId, eventObj.AuctionId);
        Assert.Equal(bid, eventObj.Bid);
        Assert.Equal($"New bid placed on auction {auctionId}. Bid Amount: {bid.BidAmount.Value}", eventObj.Message);
    }
}