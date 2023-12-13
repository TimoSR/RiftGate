using Domain.Entities;

namespace Domain.Repositories.BidEntity;

public interface IBidQueryRepository
{
    Task<Bid> GetByIdAsync(int bidId);
    Task<IEnumerable<Bid>> GetAllByAuctionIdAsync(int auctionId);
    Task<IEnumerable<Bid>> GetAllByUserIdAsync(int userId);
}