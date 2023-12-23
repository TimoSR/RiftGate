using API.Features.AuctionOperations.Domain.ValueObjects;

namespace UnitTests.AuctionListing.Domain;

public class PriceTests
{
    [Fact]
    public void Constructor_WithPositiveAmount_SetsValueCorrectly()
    {
        decimal amount = 100m;
        var price = new Price(amount);
        
        Assert.Equal(amount, price.Value);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(0)]
    public void Constructor_WithNonPositiveAmount_ThrowsArgumentException(decimal amount)
    {
        var exception = Assert.Throws<ArgumentException>(() => new Price(amount));
        Assert.Equal("BidAmount cannot be negative or zero.", exception.Message);
    }

    [Theory]
    [InlineData(10.555, 10.56)]
    [InlineData(10.554, 10.55)]
    public void Constructor_WithAmount_RoundsToTwoDecimalPlaces(decimal input, decimal expected)
    {
        var price = new Price(input);
        
        Assert.Equal(expected, price.Value);
    }
}