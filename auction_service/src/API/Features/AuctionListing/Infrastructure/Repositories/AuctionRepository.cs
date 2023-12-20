using API.Features.AuctionListing.Domain.AggregateRoots;
using API.Features.AuctionListing.Domain.Repositories;
using CodingPatterns.DomainLayer;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Persistence.MongoDB;
using MongoDB.Driver;

namespace API.Features.AuctionListing.Infrastructure.Repositories;

public class AuctionRepository<T> : MongoRepository<Auction>, IAuctionRepository
{
    protected AuctionRepository(IMongoDbManager dbManager, IDomainEventDispatcher domainEventDispatcher) : base(dbManager, domainEventDispatcher)
    {
    }
    
    public async Task<List<Auction>> GetActiveAuctionsAsync()
    {
        var collection = GetCollection();
        return await collection.Find(auction => auction.IsActive).ToListAsync();
    }
    
    public async Task<List<BuyoutAuction>> GetBuyoutAuctionsAsync()
    {
        var collection = GetCollection();
        return await collection.Find(auction => auction is BuyoutAuction)
            .ToListAsync()
            .ContinueWith(task => task.Result
                .OfType<BuyoutAuction>()
                .ToList());
    }

    public async Task<List<TraditionalAuction>> GetTraditionalAuctionsAsync()
    {
        var collection = GetCollection();
        return await collection.Find(auction => auction is TraditionalAuction)
            .ToListAsync()
            .ContinueWith(task => task.Result
                .OfType<TraditionalAuction>()
                .ToList());
    }
}