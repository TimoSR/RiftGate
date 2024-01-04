using API.Features.AuctionOperations.Domain.Repositories;
using CodingPatterns.InfrastructureLayer;

namespace API.Features.AuctionOperations.Infrastructure.CachedRepositories;

public interface ICachedAuctionRepository : ICachedRepository, IAuctionRepository
{
    
}