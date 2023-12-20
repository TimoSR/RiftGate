using API.Features.AuctionListing.Domain.AggregateRoots;
using MongoDB.Driver;

namespace API.Features.AuctionListing.Infrastructure.Repositories;

public partial class AuctionRepository
{
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

    public Task<List<Auction>> GetAuctionsBySellerIdAsync(string sellerId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Auction>> GetAuctionsEndingSoonAsync()
    {
        throw new NotImplementedException();
    }
}