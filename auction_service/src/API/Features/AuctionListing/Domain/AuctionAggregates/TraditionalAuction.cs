using API.Features._shared.Domain;
using API.Features.AuctionListing.Domain.AuctionAggregates.ValueObjects;

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