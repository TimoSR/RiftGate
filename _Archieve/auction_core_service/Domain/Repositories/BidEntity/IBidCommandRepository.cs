using Domain.Entities;

namespace Domain.Repositories.BidEntity;

public interface IBidCommandRepository
{
    Task AddAsync(Bid bid);
    Task UpdateAsync(Bid bid);
    Task DeleteAsync(int bidId);
}