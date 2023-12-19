using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;

namespace UnitTests.AuctionListing.Domain;

public class AuctionLengthTests
{
    public static IEnumerable<object[]> ValidAuctionLengths =>
        new List<object[]>
        {
            new object[] { 12 },
            new object[] { 24 },
            new object[] { 48 }
        };

    public static IEnumerable<object[]> InvalidAuctionLengths =>
        new List<object[]>
        {
            new object[] { 11 },
            new object[] { 0 },
            new object[] { -1 },
            new object[] { 49 }
        };

    [Theory]
    [MemberData(nameof(ValidAuctionLengths))]
    public void Constructor_WithValidValue_ShouldNotThrowException(int validLength)
    {
        var auctionLength = new AuctionLength(validLength);
        
        Assert.Equal(validLength, auctionLength.Value);
    }

    [Theory]
    [MemberData(nameof(InvalidAuctionLengths))]
    public void Constructor_WithInvalidValue_ShouldThrowArgumentException(int invalidLength)
    {
        var exception = Assert.Throws<ArgumentException>(() => new AuctionLength(invalidLength));
        Assert.Contains("Invalid auction length", exception.Message);
    }
}