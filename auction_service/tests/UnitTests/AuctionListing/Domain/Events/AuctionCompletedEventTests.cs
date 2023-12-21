using API.Features.AuctionListing.Domain.AuctionAggregates.Events;

namespace UnitTests.AuctionListing.Domain.Events;

public class AuctionCompletedEventTests
{
    
    private readonly DateTime _fixedDateTime = new(2023, 1, 1);
    
    [Fact]
    public void ShouldCorrectlySetProperties()
    {
        string auctionId = "Auction123";
        DateTime completionTime = _fixedDateTime;
        var eventObj = new AuctionCompletedEvent(auctionId, completionTime);

        Assert.Equal(auctionId, eventObj.AuctionId);
        Assert.Equal(completionTime, eventObj.CompletionTime);
        Assert.Equal($"Auction {auctionId} completed at {completionTime}.", eventObj.Message);
    }
}