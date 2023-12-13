using API.Features.AFeature.DomainLayer.Entities;

namespace API.Features.AFeature.DomainLayer.Repositories.AuctionEntity;

public interface IAuctionQueryRepository
{
    Task<Auction> GetByIdAsync(int auctionId);
    Task<IEnumerable<Auction>> GetAllAsync();
    Task<IEnumerable<Auction>> GetActiveAuctionsAsync();
    Task<IEnumerable<Auction>> GetAuctionsByUserIdAsync(int userId);
}