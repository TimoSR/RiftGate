using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.Events;
using API.Features.AuctionOperations.Domain.Services;
using API.Features.AuctionOperations.Domain.ValueObjects;

namespace UnitTests.AuctionOperations.Domain;

public class TraditionalAuctionTests
{
    private readonly TraditionalAuction _auction;
    private readonly Mock<ITimeService> _timeServiceMock;
    private readonly DateTime _fixedDateTime;
    private readonly Bid _validBid;
    private readonly Item _defaultItem;

    public TraditionalAuctionTests()
    {
        _fixedDateTime = new DateTime(2023, 1, 1);
        _timeServiceMock = new Mock<ITimeService>();
        _timeServiceMock.Setup(service => service.GetCurrentTime()).Returns(_fixedDateTime);

        _defaultItem = new Item(
            itemId: "default-item-id",
            name: "Default Item",
            category: "Default Category",
            group: "Default Group",
            type: "Default Type",
            rarity: "Common",
            quantity: 5
        );

        _auction = new TraditionalAuction("seller1", _defaultItem, new AuctionLength(24));
        _validBid = CreateMockedBid("bidder1", new Price(100), _fixedDateTime);

        // Use mock ITimeService when starting the auction
        _auction.StartAuction(_timeServiceMock.Object);
    }

    private Bid CreateMockedBid(string bidderId, Price bidAmount, DateTime bidTime)
    {
        return new Bid(bidderId, bidAmount, _timeServiceMock.Object);
    }

    [Fact]
    public void Constructor_WithNullSellerId_ThrowsArgumentNullException()
    {
        var exception = Record.Exception(() => new TraditionalAuction(null, _defaultItem, new AuctionLength(24)));
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Fact]
    public void Constructor_WithNullItem_ThrowsArgumentNullException()
    {
        var exception = Record.Exception(() => new TraditionalAuction("seller1", null, new AuctionLength(24)));
        Assert.NotNull(exception);
        Assert.IsType<ArgumentNullException>(exception);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Constructor_WithInvalidAuctionLength_ThrowsArgumentException(int invalidLength)
    {
        var exception = Record.Exception(() => new TraditionalAuction("seller1", _defaultItem, new AuctionLength(invalidLength)));
        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void Constructor_InitializesProperties()
    {
        Assert.Equal("seller1", _auction.SellerId);
        Assert.NotNull(_auction.Item);
        Assert.Equal(24, _auction.AuctionLengthHours.Value);
        Assert.True(_auction.IsActive);
    }

    [Fact]
    public void StartAuction_SetsPropertiesCorrectly()
    {
        Assert.Equal(_fixedDateTime, _auction.StartTime);
        Assert.True(_auction.IsActive);
        Assert.Equal(_fixedDateTime.AddHours(24), _auction.EstimatedEndTime);
    }

    [Fact]
    public void PlaceBid_OnActiveAuction_AddsBid()
    {
        _auction.PlaceBid(_validBid);
        Assert.Contains(_validBid, _auction.Bids);
        Assert.Contains(_auction.DomainEvents, e => e is AuctionStartedEvent);
        Assert.Contains(_auction.DomainEvents, e => e is BidPlacedEvent);
    }

    [Fact]
    public void PlaceBid_OnInactiveAuction_ThrowsInvalidOperationException()
    {
        var inactiveAuction = new TraditionalAuction("seller1", _defaultItem, new AuctionLength(24));
        var exception = Assert.Throws<InvalidOperationException>(() => inactiveAuction.PlaceBid(_validBid));

        // Assert that the exception is of the correct type
        Assert.IsType<InvalidOperationException>(exception);
    }

    [Fact]
    public void PlaceBid_WithLowerBidAmount_ThrowsInvalidOperationException()
    {
        var higherBidTime = _fixedDateTime.AddMinutes(10);
        _timeServiceMock.Setup(service => service.GetCurrentTime()).Returns(higherBidTime);
        var higherBid = CreateMockedBid("bidder2", new Price(150), higherBidTime);
        _auction.PlaceBid(higherBid);

        var lowerBidTime = _fixedDateTime.AddMinutes(20);
        _timeServiceMock.Setup(service => service.GetCurrentTime()).Returns(lowerBidTime);
        var lowerBid = CreateMockedBid("bidder3", new Price(90), lowerBidTime);

        var exception = Assert.Throws<InvalidOperationException>(() => _auction.PlaceBid(lowerBid));

        // Assert that the exception is of the correct type
        Assert.IsType<InvalidOperationException>(exception);
    }


    [Fact]
    public void CheckAndCompleteAuction_MarksAuctionAsComplete()
    {
        var timeAfterAuctionEnd = _fixedDateTime.AddHours(24);
        _timeServiceMock.Setup(service => service.GetCurrentTime()).Returns(timeAfterAuctionEnd);
        _auction.CheckAndCompleteAuction(_timeServiceMock.Object);
        Assert.False(_auction.IsActive);
    }

    [Fact]
    public void GetCurrentHighestBid_ReturnsHighestBid()
    {
        _auction.PlaceBid(_validBid);
        var highestBid = _auction.GetCurrentHighestBid();
        Assert.NotNull(highestBid);
        Assert.Equal(100, highestBid.BidAmount.Value);
    }
}
