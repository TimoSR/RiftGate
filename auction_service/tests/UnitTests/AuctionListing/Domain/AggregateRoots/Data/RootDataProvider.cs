using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using Moq;

namespace UnitTests.AuctionListing.Domain.AggregateRoots.Data;

public static class RootDataProvider
{
    public static IEnumerable<object[]> InvalidConstructorArguments
    {
        get
        {
            var validSellerId = "seller123";
            var validItem = new Item();
            var validAuctionLength = new AuctionLength(24);
            var validBuyout = new Price(100);
            var validTimeService = new Mock<ITimeService>().Object;

            return new List<object[]>
            {
                new object[] { null, validItem, validAuctionLength, validBuyout, validTimeService },
                new object[] { validSellerId, null, validAuctionLength, validBuyout, validTimeService },
                new object[] { validSellerId, validItem, null, validBuyout, validTimeService },
                new object[] { validSellerId, validItem, validAuctionLength, null, validTimeService },
                new object[] { validSellerId, validItem, validAuctionLength, validBuyout, null },
                // Add more combinations as needed
            };
        }
    }
}