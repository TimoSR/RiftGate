using API.Features.AuctionListing.Domain.AggregateRoots;
using API.Features.AuctionListing.Domain.AggregateRoots.AuctionAggregates.Entities;
using API.Features.AuctionListing.Domain.Repositories;
using CodingPatterns.DomainLayer;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Persistence.MongoDB;

namespace API.Features.AuctionListing.Infrastructure.Repositories;

public partial class AuctionRepository
{
    public Task CreateAuctionAsync(Auction auction)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAuctionAsync(Auction auction)
    {
        throw new NotImplementedException();
    }

    public Task<bool> SoftDeleteAuctionAsync(string auctionId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteAuctionAsync(string auctionId)
    {
        throw new NotImplementedException();
    }

    public Task<bool> PlaceBidOnAuctionAsync(string auctionId, Bid bid)
    {
        throw new NotImplementedException();
    }

    public Task<bool> CloseAuctionAsync(string auctionId)
    {
        throw new NotImplementedException();
    }
}