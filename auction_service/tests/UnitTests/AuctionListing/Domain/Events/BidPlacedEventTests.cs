using API.Features.AuctionListing.Domain.AggregateRoots.Events;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using System;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;
using Xunit;

namespace UnitTests.AuctionListing.Domain.Events;

public class BidPlacedEventTests
{
    private readonly DateTime _fixedDateTime = new DateTime(2023, 1, 1);

    [Fact]
    public void ShouldCorrectlySetProperties()
    {
        // Arrange
        string auctionId = "Auction123";
        var bidAmount = new Price(100);
        string bidderId = "Bidder123";
        var bid = new Bid(bidderId, bidAmount, _fixedDateTime);

        // Act
        var eventObj = new BidPlacedEvent(auctionId, bid);

        // Assert
        Assert.Equal(auctionId, eventObj.AuctionId);
        Assert.Equal(bid, eventObj.Bid);
        
        string expectedMessage = $"New bid placed on auction {auctionId}. " +
                                 $"Timestamp: {_fixedDateTime:yyyy-MM-dd HH:mm:ss} (UTC). " +
                                 $"Bid Amount: {bid.BidAmount.Value:C}. " +
                                 $"Bidder: {bid.BidderId}.";
        Assert.Equal(expectedMessage, eventObj.Message);
    }
}