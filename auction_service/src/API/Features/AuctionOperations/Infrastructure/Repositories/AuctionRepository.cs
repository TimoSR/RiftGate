using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.Repositories;
using CodingPatterns.DomainLayer;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Persistence.MongoDB;
using MongoDB.Bson;
using MongoDB.Driver;

namespace API.Features.AuctionOperations.Infrastructure.Repositories;

public class AuctionRepository: MongoRepository<Auction>, IAuctionRepository
{
    public AuctionRepository(
        IMongoDbManager dbManager, 
        IDomainEventDispatcher domainEventDispatcher) : base(dbManager, domainEventDispatcher) {}
    
    public IMongoCollection<Auction> GetAuctionCollection()
    {
        return GetCollection();
    }
    
    public async Task<List<Auction>> GetAllActiveAuctionsAsync()
    {
        var collection = GetCollection();

        // Filter by IsActive
        var filter = Builders<Auction>.Filter.Eq(a => a.IsActive, true);

        return await collection.Find(filter).ToListAsync();
    }

    public async Task<List<Auction>> GetActiveAuctionsAsync(int pageNumber, int pageSize)
    {
        var collection = GetCollection();

        // Calculate the number of documents to skip
        int skip = (pageNumber - 1) * pageSize;

        // Retrieve the subset of active auctions
        var auctions = await collection.Find(auction => auction.IsActive)
            .Skip(skip)
            .Limit(pageSize)
            .ToListAsync();

        return auctions;
    }

    public async Task<List<Auction>> SearchAuctionsAsync(string? name, string? category, string? group, string? type, string? rarity)
    {
        var collection = GetCollection();
        var filterBuilder = Builders<Auction>.Filter;
        var filter = filterBuilder.Empty;

        // Apply filters based on item properties
        if (!string.IsNullOrWhiteSpace(name))
        {
            filter &= filterBuilder.Regex("Item.Name", new BsonRegularExpression(name, "i"));
        }
        if (!string.IsNullOrWhiteSpace(category))
        {
            filter &= filterBuilder.Eq("Item.Category", category);
        }
        // Add similar filters for Group, Type, Rarity

        var auctions = await collection.Find(filter).ToListAsync();
        return auctions;
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

    public async Task<List<Auction>> GetAuctionsBySellerIdAsync(string sellerId)
    {
        var collection = GetCollection();
        var filter = Builders<Auction>.Filter.Eq(auction => auction.SellerId, sellerId);
        return await collection.Find(filter).ToListAsync();
    }
    
    public async Task<List<Bid>> GetAuctionBidsAsync(string auctionId)
    {
        var collection = GetCollection();
        var filter = Builders<Auction>.Filter.Eq("_id", auctionId);
        var auction = await collection.Find(filter).FirstOrDefaultAsync();
        return auction?.Bids ?? new List<Bid>();
    }
}