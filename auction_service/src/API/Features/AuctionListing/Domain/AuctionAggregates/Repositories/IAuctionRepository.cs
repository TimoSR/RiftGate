using API.Features.AuctionListing.Domain.AuctionAggregates.Entities;
using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.Domain.AuctionAggregates.Repositories;

public interface IAuctionRepository : IRepository<Auction>
{
    // Queries
    Task<List<Auction>> SearchAuctionsAsync();
    Task<List<BuyoutAuction>> GetBuyoutAuctionsAsync();
    Task<List<TraditionalAuction>> GetTraditionalAuctionsAsync();
    Task<List<Auction>> GetAuctionsBySellerIdAsync(string sellerId);
    Task<List<Auction>> GetAuctionsEndingSoonAsync();
    Task<List<Bid>> GetAuctionBidsAsync(string auctionId);
    Task<List<Bid>> GetActiveUserAuctionBids(string auctionId, string userId);
    Task<List<Bid>> GetUserBidHistoryAsync(string userId);
    
    // Commands
    Task CreateAuctionAsync(Auction auction);
    Task UpdateAuctionAsync(Auction auction);
    Task SoftDeleteAuctionAsync(string auctionId);
    Task DeleteAuctionAsync(string auctionId);
    Task PlaceBidOnAuctionAsync(string auctionId, Bid bid);
    Task CloseAuctionAsync(string auctionId);
}
