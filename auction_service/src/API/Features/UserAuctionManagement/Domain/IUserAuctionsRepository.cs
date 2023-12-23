using API.Features.AuctionManagement.Domain.Entities;

namespace API.Features.UserAuctionManagement.Domain;

public interface IUserAuctionsRepository
{
    Task<List<Bid>> GetUserBidsOnActiveAuctions(string auctionId, string userId);
    Task<List<Bid>> GetUserBidHistoryAsync(string userId);
}