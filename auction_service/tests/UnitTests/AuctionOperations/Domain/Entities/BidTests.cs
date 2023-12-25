using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.ValueObjects;

namespace UnitTests.AuctionOperations.Domain.Entities;

public class BidTests
{
    [Fact]
    public void Constructor_WithValidInputs_ShouldCreateBid()
    {
        // Arrange
        string validBidderId = "bidder123";
        var validBidAmount = new Price(100);
        var validTimeStamp = new DateTime(2023, 1, 1);

        // Act
        var bid = new Bid(validBidderId, validBidAmount, validTimeStamp);

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
        var validTimeStamp = new DateTime(2023, 1, 1);

        // Assert
        var exception = Assert.Throws(expectedException, () => new Bid(bidderId, validBidAmount, validTimeStamp));
        Assert.Contains("Bidder ID cannot be null or whitespace", exception.Message);
    }

    [Fact]
    public void Constructor_WithNullBidAmount_ShouldThrowArgumentNullException()
    {
        // Arrange
        string validBidderId = "bidder123";
        var validTimeStamp = new DateTime(2023, 1, 1);

        // Act & Assert
        var exception = Assert.Throws<ArgumentNullException>(() => new Bid(validBidderId, null, validTimeStamp));
        Assert.Equal("bidAmount", exception.ParamName);
    }
}