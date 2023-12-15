using API.Features._shared.Domain;
using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing;

public class Auction : Entity, IAggregateRoot
{
    public override int Id { get; }
    public Item Item { get; set; }
    public float Price { get; set; }
    public int AuctionLength { get; set; } = 24;
    private DateTime ListingTime => DateTime.Now;
}