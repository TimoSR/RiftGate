using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Entities;
using API.Features.AuctionOperations.Domain.Services;

namespace API.Features.AuctionOperations.Infrastructure.Repositories;

public partial class AuctionRepository
{
    public async Task CreateAuctionAsync(Auction auction)
    {
        await InsertAsync(auction);
    }

    public async Task UpdateAuctionAsync(Auction auction)
    {
        await UpdateAsync(auction);
    }

    public async Task SoftDeleteAuctionAsync(string auctionId)
    {
        var auction = await GetByIdAsync(auctionId);
        if (auction == null)
        {
            throw new KeyNotFoundException($"Auction with ID {auctionId} not found.");
        }

        await SoftDeleteAsync(auction);
    }

    public async Task DeleteAuctionAsync(string auctionId)
    {
        var auction = await GetByIdAsync(auctionId);
        if (auction == null)
        {
            throw new KeyNotFoundException($"Auction with ID {auctionId} not found.");
        }
        
        await DeleteAsync(auction);
    }

    public async Task PlaceBidOnAuctionAsync(string auctionId, Bid bid)
    {
        var auction = await GetByIdAsync(auctionId);
        if (auction == null)
        {
            throw new KeyNotFoundException($"Auction with ID {auctionId} not found.");
        }

        auction.PlaceBid(bid);
        await UpdateAsync(auction);
    }

    public async Task CloseAuctionAsync(string auctionId, ITimeService timeService)
    {
        var auction = await GetByIdAsync(auctionId);
        if (auction == null)
        {
            throw new KeyNotFoundException($"Auction with ID {auctionId} not found.");
        }

        auction.CheckAndCompleteAuction(timeService); // Assuming timeService is available
        await UpdateAsync(auction);
    }
}