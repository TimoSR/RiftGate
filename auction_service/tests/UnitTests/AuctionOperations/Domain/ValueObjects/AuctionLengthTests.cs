using API.Features.AuctionOperations.Domain.ValueObjects;
using UnitTests.AuctionListing.Domain._TestData;

namespace UnitTests.AuctionOperations.Domain.ValueObjects;

public class AuctionLengthTests
{
    [Theory]
    [MemberData(nameof(TestDataProvider.ValidAuctionLengths), MemberType = typeof(TestDataProvider))]
    public void Constructor_WithValidValue_ShouldNotThrowException(int validLength)
    {
        var auctionLength = new AuctionLength(validLength);
        
        Assert.Equal(validLength, auctionLength.Value);
    }

    [Theory]
    [MemberData(nameof(TestDataProvider.InvalidAuctionLengths), MemberType = typeof(TestDataProvider))]
    public void Constructor_WithInvalidValue_ShouldThrowArgumentException(int invalidLength)
    {
        var exception = Assert.Throws<ArgumentException>(() => new AuctionLength(invalidLength));
        Assert.Contains("Invalid auction length", exception.Message);
    }
}