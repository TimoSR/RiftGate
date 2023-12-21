using API.Features.AuctionListing.Domain.AggregateRoots;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;

namespace UnitTests.AuctionListing.Domain.AggregateRoots;

public class TraditionalAuctionTests
{
    private readonly TraditionalAuction _auction;
    private readonly DateTime _startTime;
    private readonly Bid _validBid;

    public TraditionalAuctionTests()
    {
        _startTime = DateTime.UtcNow;
        _auction = new TraditionalAuction("seller1", new Item(), new AuctionLength(24));
        _validBid = new Bid("bidder1", new Price(100), _startTime);

        _auction.StartAuction(_startTime);
    }
    
    [Fact]
    public void Constructor_WithNullSellerId_ThrowsArgumentNullException()
    {
        var exception = Record.Exception(() => new TraditionalAuction(null, new Item(), new AuctionLength(24)));

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
    // Add more cases that you consider invalid for AuctionLength
    public void Constructor_WithInvalidAuctionLength_ThrowsArgumentException(int invalidLength)
    {
        var exception = Record.Exception(() => new TraditionalAuction("seller1", new Item(), new AuctionLength(invalidLength)));

        Assert.NotNull(exception);
        Assert.IsType<ArgumentException>(exception);
    }

    [Fact]
    public void Constructor_InitializesProperties()
    {
        Assert.Equal("seller1", _auction.SellerId);
        Assert.NotNull(_auction.Item);
        Assert.Equal(24, _auction.AuctionLength.Value);
        Assert.True(_auction.IsActive);
    }

    [Fact]
    public void StartAuction_SetsPropertiesCorrectly()
    {
        Assert.Equal(_startTime, _auction.StartTime);
        Assert.True(_auction.IsActive);
        Assert.Equal(_startTime.AddHours(24), _auction.EstimatedEndTime);
    }

    [Fact]
    public void PlaceBid_OnActiveAuction_AddsBid()
    {
        _auction.PlaceBid(_validBid);
        Assert.Contains(_validBid, _auction.Bids);
    }

    [Fact]
    public void PlaceBid_OnInactiveAuction_ThrowsException()
    {
        var auction = new TraditionalAuction("seller1", new Item(), new AuctionLength(24));
        var exception = Assert.Throws<InvalidOperationException>(() => auction.PlaceBid(_validBid));
        Assert.Equal("Attempted to place a bid on an inactive auction.", exception.Message);
    }

    [Fact]
    public void PlaceBid_WithLowerBidAmount_ThrowsException()
    {
        var higherBid = new Bid("bidder2", new Price(150), _startTime);
        _auction.PlaceBid(higherBid);

        var lowerBid = new Bid("bidder3", new Price(90), _startTime);
        var exception = Assert.Throws<InvalidOperationException>(() => _auction.PlaceBid(lowerBid));
        Assert.StartsWith("Bid amount of 90 must be higher than the current highest bid of 150", exception.Message);
    }

    [Fact]
    public void CheckAndCompleteAuction_MarksAuctionAsComplete()
    {
        _auction.CheckAndCompleteAuction(_startTime.AddHours(25));
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
