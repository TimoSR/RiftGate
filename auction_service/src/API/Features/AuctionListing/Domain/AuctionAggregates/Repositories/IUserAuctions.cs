using API.Features.AuctionListing.Domain.AuctionAggregates.Entities;
using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.Domain.AuctionAggregates.Repositories;

public interface IUserAuctions
{
    Task<List<Bid>> GetUserBidsOnActiveAuctions(string auctionId, string userId);
    Task<List<Bid>> GetUserBidHistoryAsync(string userId);
}