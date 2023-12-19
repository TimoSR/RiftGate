using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.DomainService;
using Moq;

namespace UnitTests.AuctionListing.Domain._TestData;

public static class TestDataProvider
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
    
    public static IEnumerable<object[]> ValidAuctionLengths =>
        new List<object[]>
        {
            new object[] { 12 },
            new object[] { 24 },
            new object[] { 48 }
        };

    public static IEnumerable<object[]> InvalidAuctionLengths =>
        new List<object[]>
        {
            new object[] { 11 },
            new object[] { 0 },
            new object[] { -1 },
            new object[] { 49 }
        };
    
    public static IEnumerable<object[]> ConstructorTestCases
    {
        get
        {
            return new List<object[]>
            {
                new object[] { "", typeof(ArgumentException) },
                new object[] { " ", typeof(ArgumentException) },
                new object[] { null, typeof(ArgumentException) },
                new object[] { "bidder123", typeof(ArgumentNullException), "nullBidAmount" },
                new object[] { "bidder123", typeof(ArgumentNullException), "nullTimeService" }
                // Add more test cases as needed
            };
        }
    }
}