using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.Services;
using API.Features.AuctionOperations.Domain.ValueObjects;

namespace UnitTests.AuctionOperations.Domain.Entities;

public class BidTests
{
    private readonly Mock<ITimeService> _mockTimeService = new();

    [Fact]
    public void Constructor_WithValidInputs_ShouldCreateBid()
    {
        // Arrange
        string validBidderId = "bidder123";
        var validBidAmount = new Price(100);
        var validId = "generatedId";
        var validTimeStamp = DateTime.UtcNow;

        _mockTimeService.Setup(service => service.GetCurrentTime()).Returns(validTimeStamp);

        // Act
        var bid = new Bid(validBidderId, validBidAmount, _mockTimeService.Object);

        // Assert
        Assert.Equal(validBidderId, bid.BidderId);
        Assert.Equal(validBidAmount, bid.BidAmount);
        Assert.Equal(validTimeStamp, bid.TimeStamp);
    }

    [Theory]
    [InlineData(null, typeof(ArgumentException))]
    [InlineData("", typeof(ArgumentException))]
    [InlineData("   ", typeof(ArgumentException))]
    public void Constructor_WithInvalidBidderId_ShouldThrowException(string bidderId, Type expectedException)
    {
        var validBidAmount = new Price(100);
        var validTimeStamp = DateTime.UtcNow;

        _mockTimeService.Setup(service => service.GetCurrentTime()).Returns(validTimeStamp);

        // Assert
        var exception = Assert.Throws(expectedException, () => new Bid(bidderId, validBidAmount, _mockTimeService.Object));
        Assert.Contains("Bidder ID cannot be null or whitespace", exception.Message);
    }

    [Fact]
    public void Constructor_WithNullBidAmount_ShouldThrowArgumentNullException()
    {
        // Arrange
        string validBidderId = "bidder123";
        var validTimeStamp = DateTime.UtcNow;
        
        _mockTimeService.Setup(service => service.GetCurrentTime()).Returns(validTimeStamp);

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new Bid(validBidderId, null, _mockTimeService.Object));
        Assert.Equal("bidAmount", exception.ParamName);
    }
}
