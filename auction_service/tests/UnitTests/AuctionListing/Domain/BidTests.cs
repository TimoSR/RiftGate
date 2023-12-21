using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using Moq;
using UnitTests.AuctionListing.Domain._TestData;

namespace UnitTests.AuctionListing.Domain;

public class BidTests
{
    private readonly Mock<ITimeService> _mockTimeService;
    private readonly DateTime _fixedDateTime;

    public BidTests()
    {
        _fixedDateTime = new DateTime(2023, 1, 1);
        _mockTimeService = new Mock<ITimeService>();
        _mockTimeService.Setup(service => service.GetCurrentTime()).Returns(_fixedDateTime);
    }

    [Fact]
    public void Constructor_ShouldSetTimeStampFromTimeService()
    {
        // Arrange
        string validBidderId = "bidder123";
        var validBidAmount = new Price(100);

        // Act
        var bid = new Bid(validBidderId, validBidAmount, _mockTimeService.Object);

        // Assert
        Assert.Equal(_fixedDateTime, bid.TimeStamp);
    }
    
    [Fact]
    public void Constructor_WithValidInputs_ShouldCreateBid()
    {
        var bidAmount = new Price(100);
        var bid = new Bid("bidder123", bidAmount, _mockTimeService.Object);

        Assert.Equal("bidder123", bid.BidderId);
        Assert.Equal(bidAmount, bid.BidAmount);
        Assert.Equal(_fixedDateTime, bid.TimeStamp);
    }

    [Theory]
    [MemberData(nameof(TestDataProvider.ConstructorTestCases), MemberType = typeof(TestDataProvider))]
    public void Constructor_WithInvalidArguments_ShouldThrowException(
        string bidderId, Type expectedException, string scenario = "")
    {
        var validBidAmount = new Price(100);
        ITimeService timeService = scenario == "nullTimeService" ? null : _mockTimeService.Object;
        Price bidAmount = scenario == "nullBidAmount" ? null : validBidAmount;

        // Assert that the expected exception is thrown
        Assert.Throws(expectedException, () => new Bid(bidderId, bidAmount, timeService));
    }
}