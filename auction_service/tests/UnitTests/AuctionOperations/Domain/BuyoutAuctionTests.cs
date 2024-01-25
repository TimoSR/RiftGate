using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.Events;
using API.Features.AuctionOperations.Domain.Services;
using API.Features.AuctionOperations.Domain.ValueObjects;

namespace UnitTests.AuctionOperations.Domain;

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
        
        _item = new Item(
            itemId: "default-item-id",
            name: "Default Item Name",
            category: "Default Category",
            group: "Default Group",
            type: "Default Type",
            rarity: "Common",
            quantity: 10
        );
        
        _auctionLength = new AuctionLength(24);
        _sellerId = "seller123";
        _validBid = CreateMockedBid("bidder1", new Price(50), _fixedDateTime);
    }

    private Bid CreateMockedBid(string bidderId, Price bidAmount, DateTime bidTime)
    {
        return new Bid(bidderId, bidAmount, _timeServiceMock.Object);
    }

    [Fact]
    public void Constructor_InitializesPropertiesCorrectly()
    {
        var auction = new BuyoutAuction(_sellerId, _item, _auctionLength, _buyoutPrice);

        Assert.Equal(_sellerId, auction.SellerId);
        Assert.Equal(_item, auction.Item);
        Assert.Equal(_auctionLength, auction.AuctionLengthHours);
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
    public void PlaceBid_LowerThanHighest_ThrowsInvalidOperationException()
    {
        var auction = new BuyoutAuction(_sellerId, _item, _auctionLength, _buyoutPrice);
        auction.StartAuction(_timeServiceMock.Object);
        var highBid = CreateMockedBid("bidder2", new Price(60), _fixedDateTime);
        auction.PlaceBid(highBid);

        var lowBid = CreateMockedBid("bidder1", new Price(55), _fixedDateTime);

        var exception = Assert.Throws<InvalidOperationException>(() => auction.PlaceBid(lowBid));

        // Assert that the exception is of the correct type
        Assert.IsType<InvalidOperationException>(exception);
    }


    [Fact]
    public void PlaceBid_MeetsOrExceedsBuyout_ShouldCompleteAuction()
    {
        var auction = new BuyoutAuction(_sellerId, _item, _auctionLength, _buyoutPrice);
        auction.StartAuction(_timeServiceMock.Object);
        var bid = CreateMockedBid("bidder1", _buyoutPrice, _fixedDateTime.AddHours(2));

        auction.PlaceBid(bid);

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

        // Assert that the exception is of the correct type
        Assert.IsType<InvalidOperationException>(exception);
    }


    [Fact]
    public void PlaceBid_LowerThanBuyoutButHigherThanHighest_AcceptsBid()
    {
        var auction = new BuyoutAuction(_sellerId, _item, _auctionLength, _buyoutPrice);
        auction.StartAuction(_timeServiceMock.Object);
        var bid = CreateMockedBid("bidder1", new Price(80), _fixedDateTime.AddHours(24));

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
