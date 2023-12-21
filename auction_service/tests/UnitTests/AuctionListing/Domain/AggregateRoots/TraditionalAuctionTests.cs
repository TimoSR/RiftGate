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
    private readonly DateTime _fixedDateTime = new(2023, 1, 1);

    private TraditionalAuction CreateAuction()
    {
        return new TraditionalAuction("seller123", new Item(), new AuctionLength(24));
    }
    
    [Fact]
    public void PlaceBid_ValidBid_ShouldRaiseBidPlacedEvent()
    {
        var auction = CreateAuction();
        auction.StartAuction(_fixedDateTime);
        var bid = new Bid("bidder123", new Price(100),_fixedDateTime);

        auction.PlaceBid(bid);

        Assert.Contains(auction.DomainEvents, e => e is BidPlacedEvent);
    }


}