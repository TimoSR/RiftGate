using CodingPatterns.DomainLayer;
using MongoDB.Bson;

namespace API.Features.AuctionOperations.Domain.Services;

public class IdService : IIdService, IDomainService
{
    public string GenerateId()
    {
        return ObjectId.GenerateNewId().ToString();
    }
}