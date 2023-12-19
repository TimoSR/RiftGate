using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using Moq;

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
        string validBidderId = "bidder123";
        var validBidAmount = new Price(100);

        var bid = new Bid(validBidderId, validBidAmount, _mockTimeService.Object);

        Assert.Equal(validBidderId, bid.BidderId);
        Assert.Equal(validBidAmount, bid.BidAmount);
        Assert.True(bid.TimeStamp <= DateTime.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData(null)]
    public void Constructor_WithInvalidBidderId_ShouldThrowArgumentException(string invalidBidderId)
    {
        var bidAmount = new Price(100);

        // Assert that an ArgumentException is thrown, regardless of the message
        Assert.Throws<ArgumentException>(() => new Bid(invalidBidderId, bidAmount, _mockTimeService.Object));
    }

    [Fact]
    public void Constructor_WithNullBidAmount_ShouldThrowArgumentNullException()
    {
        string validBidderId = "bidder123";

        // Assert that an ArgumentNullException is thrown
        Assert.Throws<ArgumentNullException>(() => new Bid(validBidderId, null, _mockTimeService.Object));
    }

}