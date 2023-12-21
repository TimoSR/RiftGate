using API.Features.AuctionListing.Domain.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.AuctionAggregates.Events;
using API.Features.AuctionListing.Domain.AuctionAggregates.ValueObjects;

namespace UnitTests.AuctionListing.Domain.Events;

public class AuctionEventsTests
{
    [Fact]
    public void AuctionCompletedEvent_CreatesCorrectMessage()
    {
        var completionTime = new DateTime(2023, 1, 1);
        var eventObj = new AuctionCompletedEvent("Auction123", completionTime);

        var expectedMessage = $"Auction Auction123 completed at {completionTime:yyyy-MM-dd HH:mm:ss} (UTC).";
        Assert.Equal(expectedMessage, eventObj.Message);
    }

    [Fact]
    public void AuctionStartedEvent_CreatesCorrectMessage()
    {
        var startTime = new DateTime(2023, 1, 1);
        var eventObj = new AuctionStartedEvent("Auction123", startTime);

        var expectedMessage = $"Auction Auction123 started at {startTime:yyyy-MM-dd HH:mm:ss} (UTC).";
        Assert.Equal(expectedMessage, eventObj.Message);
    }

    [Fact]
    public void BidPlacedEvent_CreatesCorrectMessage()
    {
        var bidTime = new DateTime(2023, 1, 1);
        var bid = new Bid("Bidder123", new Price(100), bidTime);
        var eventObj = new BidPlacedEvent("Auction123", bid);

        var expectedMessage = 
            $"New bid placed on auction Auction123. " +
            $"Timestamp: {bidTime:yyyy-MM-dd HH:mm:ss} (UTC). " +
            $"Bid Amount: {bid.BidAmount.Value:C}. " +
            $"Bidder: Bidder123.";
        Assert.Equal(expectedMessage, eventObj.Message);
    }

    [Fact]
    public void AuctionSoldEvent_CreatesCorrectMessage()
    {
        var sellingTime = new DateTime(2023, 1, 1);
        var winningBid = new Bid("Bidder123", new Price(150), sellingTime);
        var eventObj = new AuctionSoldEvent("Auction123", winningBid, sellingTime);

        var expectedMessage = 
            $"Auction 'Auction123' was successfully sold to bidder 'Bidder123' " +
            $"with a winning bid of {winningBid.BidAmount.Value:C} at {sellingTime:yyyy-MM-dd HH:mm:ss} (UTC).";
        Assert.Equal(expectedMessage, eventObj.Message);
    }

    [Fact]
    public void AuctionExpiredEvent_CreatesCorrectMessage()
    {
        var expiredTime = new DateTime(2023, 1, 1);
        var eventObj = new AuctionExpiredEvent("Auction123", expiredTime);

        var expectedMessage = 
            $"Auction 'Auction123' has expired without a winning bid at {expiredTime:yyyy-MM-dd HH:mm:ss} (UTC).";
        Assert.Equal(expectedMessage, eventObj.Message);
    }
}