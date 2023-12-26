using CodingPatterns.DomainLayer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Features.AuctionOperations.Domain.ValueObjects;

public record Price : IValueObject
{
    [BsonRepresentation(BsonType.Decimal128)]
    public decimal Value { get; private set; }

    public Price(decimal bidAmount)
    {
        if (bidAmount <= 0) throw new ArgumentException("bidAmount cannot be negative or zero.");

        Value = decimal.Round(bidAmount, 2);
    }
}
