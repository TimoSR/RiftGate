using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.Services;
using API.Features.AuctionOperations.Domain.ValueObjects;

namespace UnitTests.AuctionOperations.Domain.Entities;

public class BidTests
{
    private readonly Mock<IIdService> _mockIdService = new();
    private readonly Mock<ITimeService> _mockTimeService = new();

    [Fact]
    public void Constructor_WithValidInputs_ShouldCreateBid()
    {
        // Arrange
        string validBidderId = "bidder123";
        var validBidAmount = new Price(100);
        var validId = "generatedId";
        var validTimeStamp = DateTime.UtcNow;

        _mockIdService.Setup(service => service.GenerateId()).Returns(validId);
        _mockTimeService.Setup(service => service.GetCurrentTime()).Returns(validTimeStamp);

        // Act
        var bid = new Bid(_mockIdService.Object, validBidderId, validBidAmount, _mockTimeService.Object);

        // Assert
        Assert.Equal(validId, bid.Id);
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
        var validId = "generatedId";
        var validTimeStamp = DateTime.UtcNow;

        _mockIdService.Setup(service => service.GenerateId()).Returns(validId);
        _mockTimeService.Setup(service => service.GetCurrentTime()).Returns(validTimeStamp);

        // Assert
        var exception = Assert.Throws(expectedException, () => new Bid(_mockIdService.Object, bidderId, validBidAmount, _mockTimeService.Object));
        Assert.Contains("Bidder ID cannot be null or whitespace", exception.Message);
    }

    [Fact]
    public void Constructor_WithNullBidAmount_ShouldThrowArgumentNullException()
    {
        // Arrange
        string validBidderId = "bidder123";
        var validId = "generatedId";
        var validTimeStamp = DateTime.UtcNow;

        _mockIdService.Setup(service => service.GenerateId()).Returns(validId);
        _mockTimeService.Setup(service => service.GetCurrentTime()).Returns(validTimeStamp);

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new Bid(_mockIdService.Object, validBidderId, null, _mockTimeService.Object));
        Assert.Equal("bidAmount", exception.ParamName);
    }
}
