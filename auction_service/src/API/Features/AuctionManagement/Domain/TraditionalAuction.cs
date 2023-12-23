using API.Features.AuctionListing.Domain.AuctionAggregates.ValueObjects;
using API.Features.AuctionManagement.Domain.Entities;

namespace API.Features.AuctionListing.Domain.AuctionAggregates;

    public class TraditionalAuction : Auction
    {
        public TraditionalAuction(
            string sellerId, 
            Item item, 
            AuctionLength auctionLength) 
            : base(sellerId, item, auctionLength)
        {
        }
    }