using MongoDB.Bson;

namespace API.Features.AuctionOperations.Domain.Services;

public interface IIdService
{
    string GenerateId();
}