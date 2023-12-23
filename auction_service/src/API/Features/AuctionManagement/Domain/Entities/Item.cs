using CodingPatterns.DomainLayer;

namespace API.Features.AuctionManagement.Domain.Entities;

// Modified Item class
public class Item : Entity
{
    public string Name { get; set; }
    public string Category { get; set; }
    public string Group { get; set; }
    public string Type { get; set; }
    public string Rarity { get; set; }
    public int Quantity { get; set; }
}

