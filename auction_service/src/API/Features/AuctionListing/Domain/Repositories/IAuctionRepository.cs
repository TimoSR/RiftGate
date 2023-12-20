using API.Features.AuctionListing.Domain.AggregateRoots;
using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.Domain.Repositories;

public interface IAuctionRepository : IRepository<Auction>
{
    // Add any additional methods specific to the auction domain
    Task<List<BuyoutAuction>> GetBuyoutAuctionsAsync();
    Task<List<TraditionalAuction>> GetTraditionalAuctionsAsync();
    // Other auction-specific methods...
}
