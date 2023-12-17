using CodingPatterns.DomainLayer;

namespace API.Features._shared.Domain;

// Modified Item class
public class Item : Entity
{
    public override string Id { get; }
    public string Name { get; set; }
    public string Category { get; set; }
    public string Group { get; set; }
    public string Type { get; set; }
    public string Rarity { get; set; }
    public int Quantity { get; set; }
}

