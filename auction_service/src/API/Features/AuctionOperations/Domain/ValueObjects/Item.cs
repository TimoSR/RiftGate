using CodingPatterns.DomainLayer;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Features.AuctionOperations.Domain.ValueObjects;

// Modified Item class
public class Item : IValueObject
{
    [BsonElement("itemId")]
    public string ItemId { get; private set;}
    [BsonElement("name")]
    public string Name { get; private set; }

    [BsonElement("category")]
    public string Category { get; private set; }

    [BsonElement("group")]
    public string Group { get; private set; }

    [BsonElement("type")]
    public string Type { get; private set; }

    [BsonElement("rarity")]
    public string Rarity { get; private set; }

    [BsonElement("quantity")]
    public int Quantity { get; private set; }

    public Item(string itemId, string name, string category, string group, string type, string rarity, int quantity)
    {
        ItemId = itemId ?? throw new ArgumentNullException(nameof(itemId));
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Category = category ?? throw new ArgumentNullException(nameof(category));
        Group = group ?? throw new ArgumentNullException(nameof(group));
        Type = type ?? throw new ArgumentNullException(nameof(type));
        Rarity = rarity ?? throw new ArgumentNullException(nameof(rarity));
        Quantity = quantity > 0 ? quantity : throw new ArgumentException("quantity must be greater than zero.", nameof(quantity));
    }
}

