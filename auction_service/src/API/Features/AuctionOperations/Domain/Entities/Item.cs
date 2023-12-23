using CodingPatterns.DomainLayer;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Features.AuctionOperations.Domain.Entities;

// Modified Item class
public class Item : Entity
{
    [BsonElement("name")]
    public string Name { get; set; }

    [BsonElement("category")]
    public string Category { get; set; }

    [BsonElement("group")]
    public string Group { get; set; }

    [BsonElement("type")]
    public string Type { get; set; }

    [BsonElement("rarity")]
    public string Rarity { get; set; }

    [BsonElement("quantity")]
    public int Quantity { get; set; }
}

