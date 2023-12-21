using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.AggregateRoots.Events;
using Moq;
using UnitTests.AuctionListing.Domain.AggregateRoots.Data;

namespace UnitTests.AuctionListing.Domain.AggregateRoots;

public class TraditionalAuctionTests
{
    private readonly Mock<ITimeService> _mockTimeService;
    private readonly DateTime _fixedDateTime;

    public TraditionalAuctionTests()
    {
        _fixedDateTime = new DateTime(2023, 1, 1);
        _mockTimeService = new Mock<ITimeService>();
        _mockTimeService.Setup(service => service.GetCurrentTime()).Returns(_fixedDateTime);
    }
    
    private TraditionalAuction CreateAuction()
    {
        return new TraditionalAuction("seller123", new Item(), new AuctionLength(24), _mockTimeService.Object);
    }
    
    [Theory]
    [MemberData(nameof(RootDataProvider.InvalidConstructorArguments), MemberType = typeof(RootDataProvider))]
    public void TraditionalAuction_Constructor_WithInvalidArguments_ShouldThrowException(
        string sellerId, Item item, AuctionLength auctionLength, Price buyout, ITimeService timeService)
    {
        Assert.Throws<ArgumentNullException>(() => new BuyoutAuction(sellerId, item, auctionLength, buyout, timeService));
    }
    
    [Fact]
    public void PlaceBid_ValidBid_ShouldRaiseBidPlacedEvent()
    {
        var auction = CreateAuction();
        auction.StartAuction();
        var bid = new Bid("bidder123", new Price(100), _mockTimeService.Object);

        auction.PlaceBid(bid);

        Assert.Contains(auction.DomainEvents, e => e is BidPlacedEvent);
    }


}