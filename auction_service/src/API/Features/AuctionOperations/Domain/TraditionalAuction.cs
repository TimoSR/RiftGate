using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.ValueObjects;

namespace API.Features.AuctionOperations.Domain;

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