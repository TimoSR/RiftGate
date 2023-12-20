using API.Features.AuctionListing.Domain.AggregateRoots;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.Domain.Repositories;

public interface IAuctionRepository : IRepository<Auction>
{
    
    // Queries
    Task<List<BuyoutAuction>> GetBuyoutAuctionsAsync();
    Task<List<TraditionalAuction>> GetTraditionalAuctionsAsync();
    Task<List<Auction>> GetAuctionsBySellerIdAsync(string sellerId);
    Task<List<Auction>> GetAuctionsEndingSoonAsync();   
    
    // Commands
    Task CreateAuctionAsync(Auction auction);
    Task UpdateAuctionAsync(Auction auction);
    Task<bool> DeleteAuctionAsync(string auctionId);
    Task<bool> PlaceBidOnAuctionAsync(string auctionId, Bid bid);
    Task<bool> CloseAuctionAsync(string auctionId);
}
