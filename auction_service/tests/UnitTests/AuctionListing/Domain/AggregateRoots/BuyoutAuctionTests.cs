using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using API.Features.AuctionListing.Domain.AggregateRoots.Events;
using Moq;
using UnitTests.AuctionListing.Domain.AggregateRoots.Data;

namespace UnitTests.AuctionListing.Domain.AggregateRoots;

public class BuyoutAuctionTests
{
    private readonly Mock<ITimeService> _mockTimeService;
    private readonly DateTime _fixedDateTime;

    public BuyoutAuctionTests()
    {
        _fixedDateTime = new DateTime(2023, 1, 1);
        _mockTimeService = new Mock<ITimeService>();
        _mockTimeService.Setup(service => service.GetCurrentTime()).Returns(_fixedDateTime);
    }

    [Theory]
    [MemberData(nameof(RootDataProvider.InvalidConstructorArguments), MemberType = typeof(RootDataProvider))]
    public void BuyoutAuction_Constructor_WithInvalidArguments_ShouldThrowException(
        string sellerId, Item item, AuctionLength auctionLength, Price buyout, ITimeService timeService)
    {
        Assert.Throws<ArgumentNullException>(() => new BuyoutAuction(sellerId, item, auctionLength, buyout, timeService));
    }
    
    [Fact]
    public void StartAuction_ShouldSetStartTimeAndRaiseEvent()
    {
        var auction = new BuyoutAuction("seller123", new Item(), new AuctionLength(24), new Price(100), _mockTimeService.Object);

        auction.StartAuction();

        Assert.True(auction.IsActive);
        Assert.Contains(auction.DomainEvents, e => e is AuctionStartedEvent);
    }
    
    [Fact]
    public void CompleteAuction_ShouldSetIsActiveToFalseAndRaiseEvent()
    {
        var auction = new BuyoutAuction("seller123", new Item(), new AuctionLength(24), new Price(100), _mockTimeService.Object);

        auction.StartAuction();
        auction.CompleteAuction();

        Assert.False(auction.IsActive);
        Assert.Contains(auction.DomainEvents, e => e is AuctionCompletedEvent);
    }
}