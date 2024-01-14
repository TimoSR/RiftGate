using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.Repositories;
using CodingPatterns.InfrastructureLayer;
using Infrastructure.Persistence.Redis;

namespace API.Features.AuctionOperations.Infrastructure.CachedRepositories;

public class CachedAuctionRepository : CachedRepositoryDecorator<Auction>, ICachedAuctionRepository
{
    private readonly IAuctionRepository _repository;
    
    public CachedAuctionRepository(IAuctionRepository repository, ICacheManager cacheManager) : base(repository, cacheManager)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public Task<List<Auction>> GetAllActiveAuctionsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Auction>> GetActiveAuctionsAsync(int pageNumber, int pageSize)
    {
        throw new NotImplementedException();
    }

    public Task<List<Auction>> SearchAuctionsAsync(string? name, string? category, string? group, string? type, string? rarity)
    {
        throw new NotImplementedException();
    }

    public Task<List<BuyoutAuction>> GetBuyoutAuctionsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<TraditionalAuction>> GetTraditionalAuctionsAsync()
    {
        throw new NotImplementedException();
    }

    public Task<List<Auction>> GetAuctionsBySellerIdAsync(string sellerId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Bid>> GetAuctionBidsAsync(string auctionId)
    {
        throw new NotImplementedException();
    }
}