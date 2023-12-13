using Domain.Entities;

namespace Domain.Repositories.AuctionEntity;

public interface IAuctionCommandRepository
{
    Task AddAsync(Auction auction);
    Task UpdateAsync(Auction auction);
    Task DeleteAsync(int auctionId);
}