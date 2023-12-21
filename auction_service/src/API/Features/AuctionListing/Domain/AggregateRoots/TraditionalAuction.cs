    using API.Features._shared.Domain;
    using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates;

    namespace API.Features.AuctionListing.Domain.AggregateRoots;

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