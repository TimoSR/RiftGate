using API.Features.AuctionListing.Domain.AuctionAggregates.Entities;
using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.Domain.AuctionAggregates.Repositories;

public interface IAuctionRepository : IRepository<Auction>
{
    // Queries
    Task<List<Auction>> GetAllActiveAuctionsAsync();
    Task<List<Auction>> GetActiveAuctionsAsync(int pageNumber, int pageSize);
    Task<List<Auction>> SearchAuctionsAsync(string? name, string? category, string? group, string? type,
        string? rarity);
    Task<List<BuyoutAuction>> GetBuyoutAuctionsAsync();
    Task<List<TraditionalAuction>> GetTraditionalAuctionsAsync();
    Task<List<Auction>> GetAuctionsBySellerIdAsync(string sellerId);
    Task<List<Auction>> GetAuctionsEndingSoonAsync();
    Task<List<Bid>> GetAuctionBidsAsync(string auctionId);
}
