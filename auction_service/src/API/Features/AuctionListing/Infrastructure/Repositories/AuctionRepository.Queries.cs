using API.Features.AuctionListing.Domain.AuctionAggregates;
using API.Features.AuctionListing.Domain.AuctionAggregates.Entities;
using MongoDB.Driver;

namespace API.Features.AuctionListing.Infrastructure.Repositories;

public partial class AuctionRepository
{
    public async Task<List<Auction>> GetAllActiveAuctionsAsync()
    {
        var collection = GetCollection();
        return await collection.Find(auction => auction.IsActive).ToListAsync();
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

    public Task<List<Auction>> SearchAuctionsAsync()
    {
        throw new NotImplementedException();
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

    public Task<List<Bid>> GetAuctionBidsAsync(string auctionId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Bid>> GetActiveUserAuctionBids(string auctionId, string userId)
    {
        throw new NotImplementedException();
    }

    public Task<List<Bid>> GetUserBidHistoryAsync(string userId)
    {
        throw new NotImplementedException();
    }
}