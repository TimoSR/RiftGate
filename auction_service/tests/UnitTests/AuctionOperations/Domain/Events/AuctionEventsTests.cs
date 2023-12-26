using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.Events;
using API.Features.AuctionOperations.Domain.Services;
using API.Features.AuctionOperations.Domain.ValueObjects;
using Moq;

namespace UnitTests.AuctionOperations.Domain.Events;

public class AuctionEventsTests
{
    private readonly Mock<IIdService> _mockIdService = new();
    private readonly Mock<ITimeService> _mockTimeService = new();

    private Bid CreateMockedBid(string bidderId, Price bidAmount, DateTime bidTime)
    {
        var validId = "generatedId";
        _mockIdService.Setup(service => service.GenerateId()).Returns(validId);
        _mockTimeService.Setup(service => service.GetCurrentTime()).Returns(bidTime);

        return new Bid(_mockIdService.Object, bidderId, bidAmount, _mockTimeService.Object);
    }

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
        var bid = CreateMockedBid("Bidder123", new Price(100), bidTime);
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
        var winningBid = CreateMockedBid("Bidder123", new Price(150), sellingTime);
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
