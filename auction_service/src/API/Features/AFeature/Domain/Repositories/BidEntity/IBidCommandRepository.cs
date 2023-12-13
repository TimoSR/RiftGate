using API.Features.AFeature.DomainLayer.Entities;

namespace API.Features.AFeature.DomainLayer.Repositories.BidEntity;

public interface IBidCommandRepository
{
    Task AddAsync(Bid bid);
    Task UpdateAsync(Bid bid);
    Task DeleteAsync(int bidId);
}