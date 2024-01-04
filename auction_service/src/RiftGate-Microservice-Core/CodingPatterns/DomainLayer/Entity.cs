using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace CodingPatterns.DomainLayer;

public class Entity : IEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; protected init; } = ObjectId.GenerateNewId().ToString();
}