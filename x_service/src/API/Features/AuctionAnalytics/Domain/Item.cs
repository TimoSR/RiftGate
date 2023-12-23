using CodingPatterns.DomainLayer;

namespace API.Features.AuctionAnalytics.Domain;

// Modified Item class
public class Item : Entity
{
    public string Name { get; set; }
    public string Category { get; set; }
    public string Group { get; set; }
    public string Type { get; set; }
}

