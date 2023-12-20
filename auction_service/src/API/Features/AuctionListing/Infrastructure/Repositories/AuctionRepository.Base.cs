using API.Features.AuctionListing.Domain.AggregateRoots;
using API.Features.AuctionListing.Domain.Repositories;
using CodingPatterns.DomainLayer;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Persistence.MongoDB;

namespace API.Features.AuctionListing.Infrastructure.Repositories;

public partial class AuctionRepository: MongoRepository<Auction>, IAuctionRepository
{
    public AuctionRepository(IMongoDbManager dbManager, IDomainEventDispatcher domainEventDispatcher) : base(dbManager, domainEventDispatcher)
    {
    }
}