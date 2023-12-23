using API.Features.AuctionOperations.Domain.Entities;

namespace API.Features.UserTradingActivity.Domain.Repositories;

public interface IUserAuctionBidsRepository
{
    Task<List<Bid>> GetUserBidsOnActiveAuctions(string auctionId, string userId);
    Task<List<Bid>> GetUserBidHistoryAsync(string userId);
}