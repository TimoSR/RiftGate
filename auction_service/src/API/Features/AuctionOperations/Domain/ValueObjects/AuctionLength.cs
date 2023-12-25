using CodingPatterns.DomainLayer;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Features.AuctionOperations.Domain.ValueObjects;

public class AuctionLength : IValueObject
{
    [BsonElement("value")]
    public int Value { get; private set; }
    private readonly int[] _allowedValues = { 12, 24, 48 };

    public AuctionLength(int value)
    {
        if (!_allowedValues.Contains(value))
        {
            throw new ArgumentException(
                $"Invalid auction length. Allowed values are: {string.Join(", ", _allowedValues)}.");
        }

        Value = value;
    }
}