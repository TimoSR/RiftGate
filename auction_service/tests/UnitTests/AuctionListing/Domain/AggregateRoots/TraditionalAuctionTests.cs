using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;

namespace UnitTests.AuctionListing.Domain.AggregateRoots;

public class TraditionalAuctionTests
{
    private readonly DateTime _fixedDateTime = new(2023, 1, 1);

    private TraditionalAuction CreateAuction()
    {
        return new TraditionalAuction("seller123", new Item(), new AuctionLength(24));
    }

}