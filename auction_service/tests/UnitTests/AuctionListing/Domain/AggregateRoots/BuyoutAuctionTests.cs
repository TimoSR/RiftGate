using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AuctionAggregates;
using API.Features.AuctionListing.Domain.AuctionAggregates.DomainService;
using API.Features.AuctionListing.Domain.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.AuctionAggregates.Events;
using API.Features.AuctionListing.Domain.AuctionAggregates.ValueObjects;
using Moq;

namespace UnitTests.AuctionListing.Domain.AggregateRoots;

public class BuyoutAuctionTests
{
    private readonly DateTime _fixedDateTime;
    private readonly Mock<ITimeService> _timeServiceMock;
    private readonly Price _buyoutPrice;
    private readonly Item _item;
    private readonly AuctionLength _auctionLength;
    private readonly string _sellerId;
    private readonly Bid _validBid;

    public BuyoutAuctionTests()
    {
        _fixedDateTime = new DateTime(2023, 1, 1);
        _timeServiceMock = new Mock<ITimeService>();
        _timeServiceMock.Setup(service => service.GetCurrentTime()).Returns(_fixedDateTime);

        _buyoutPrice = new Price(100);
        _item = new Item();
        _auctionLength = new AuctionLength(24);
        _sellerId = "seller123";
        _validBid = new Bid("bidder1", new Price(50), _fixedDateTime);
    }

    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        var auction = new BuyoutAuction(_sellerId, _item, _auctionLength, _buyoutPrice);

        Assert.Equal(_sellerId, auction.SellerId);
        Assert.Equal(_item, auction.Item);
        Assert.Equal(_auctionLength, auction.AuctionLength);
        Assert.Equal(_buyoutPrice, auction.BuyoutAmount);
        Assert.False(auction.IsActive);
    }
    
    [Fact]
    public void PlaceValidBid_ShouldAddBidToList()
    {
        var auction = new BuyoutAuction(_sellerId, _item, _auctionLength, _buyoutPrice);
        auction.StartAuction(_timeServiceMock.Object);

        auction.PlaceBid(_validBid);

        Assert.Contains(_validBid, auction.Bids);
        Assert.Contains(auction.DomainEvents, e => e is BidPlacedEvent);
    }

    [Fact]
    public void PlaceBid_LowerThanHighest_ShouldThrowException()
    {
        var auction = new BuyoutAuction(_sellerId, _item, _auctionLength, _buyoutPrice);
        auction.StartAuction(_timeServiceMock.Object);
        var highBid = new Bid("bidder2", new Price(60), _timeServiceMock.Object.GetCurrentTime());
        auction.PlaceBid(highBid);

        var lowBid = new Bid("bidder1", new Price(55), _timeServiceMock.Object.GetCurrentTime());

        var exception = Assert.Throws<InvalidOperationException>(() => auction.PlaceBid(lowBid));
        Assert.Equal("Bid amount of 55 must be higher than the current highest bid of 60.", exception.Message);
    }

    [Fact]
    public void PlaceBid_MeetsOrExceedsBuyout_ShouldCompleteAuction()
    {
        // Arrange
        var auction = new BuyoutAuction(_sellerId, _item, _auctionLength, _buyoutPrice);
        auction.StartAuction(_timeServiceMock.Object);
        var bid = new Bid("bidder1", _buyoutPrice, _fixedDateTime.AddHours(2));

        // Act
        auction.PlaceBid(bid);

        // Assert
        Assert.False(auction.IsActive);
        Assert.Contains(auction.DomainEvents, e => e is BidPlacedEvent);
        Assert.Contains(auction.DomainEvents, e => e is AuctionSoldEvent);
    }
    
    [Fact]
    public void PlaceBid_NullBid_ThrowsArgumentNullException()
    {
        var auction = new BuyoutAuction(_sellerId, _item, _auctionLength, _buyoutPrice);

        Assert.Throws<ArgumentNullException>(() => auction.PlaceBid(null));
    }

    [Fact]
    public void PlaceBid_OnInactiveAuction_ThrowsInvalidOperationException()
    {
        var auction = new BuyoutAuction(_sellerId, _item, _auctionLength, _buyoutPrice);

        var exception = Assert.Throws<InvalidOperationException>(() => auction.PlaceBid(_validBid));
        Assert.Equal("Attempted to place a bid on an inactive auction.", exception.Message);
    }

    [Fact]
    public void PlaceBid_LowerThanBuyoutButHigherThanHighest_AcceptsBid()
    {
        var auction = new BuyoutAuction(_sellerId, _item, _auctionLength, _buyoutPrice);
        auction.StartAuction(_timeServiceMock.Object);
        var bid = new Bid("bidder1", new Price(80), _fixedDateTime.AddHours(24));

        auction.PlaceBid(bid);

        Assert.Contains(bid, auction.Bids);
    }

    [Fact]
    public void Auction_ExpiresWithoutBuyout_IsNotActive()
    {
        var auction = new BuyoutAuction(_sellerId, _item, _auctionLength, _buyoutPrice);
        auction.StartAuction(_timeServiceMock.Object);

        var timeAfterAuctionEnd = _fixedDateTime.AddHours(25);
        _timeServiceMock.Setup(service => service.GetCurrentTime()).Returns(timeAfterAuctionEnd);

        auction.CheckAndCompleteAuction(_timeServiceMock.Object);

        Assert.False(auction.IsActive);
    }
}