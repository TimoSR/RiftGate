using API.Features.AFeature.DomainLayer.Entities;

namespace API.Features.AFeature.DomainLayer.Repositories.BidEntity;

public interface IBidQueryRepository
{
    Task<Bid> GetByIdAsync(int bidId);
    Task<IEnumerable<Bid>> GetAllByAuctionIdAsync(int auctionId);
    Task<IEnumerable<Bid>> GetAllByUserIdAsync(int userId);
}