namespace API.Features.AFeature.DomainLayer.Entities;

public class Item
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    // We should add a duration 12, 24, 48
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal StartingPrice { get; set; }

    public virtual ICollection<Auction> Auctions { get; set; } // Collection of Auctions
}