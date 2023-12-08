using Domain.Entities;

namespace Domain.Repositories.AuctionEntity;

public interface IAuctionQueryRepository
{
    Task<Auction> GetByIdAsync(int auctionId);
    Task<IEnumerable<Auction>> GetAllAsync();
    Task<IEnumerable<Auction>> GetActiveAuctionsAsync();
    Task<IEnumerable<Auction>> GetAuctionsByUserIdAsync(int userId);
}