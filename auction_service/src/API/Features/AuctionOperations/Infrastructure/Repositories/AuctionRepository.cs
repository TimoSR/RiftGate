using API.Features.AuctionOperations.Domain;
using API.Features.AuctionOperations.Domain.Repositories;
using CodingPatterns.DomainLayer;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Persistence.MongoDB;

namespace API.Features.AuctionOperations.Infrastructure.Repositories;

public partial class AuctionRepository: MongoRepository<Auction>, IAuctionRepository
{
    public AuctionRepository(IMongoDbManager dbManager, IDomainEventDispatcher domainEventDispatcher) : base(dbManager, domainEventDispatcher)
    {
    }
}