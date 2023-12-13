namespace API.Features.AFeature.DomainLayer.Entities;

public class Auction
{
    public int AuctionId { get; set; }
    // We should add a duration 12, 24, 48
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int ItemId { get; set; }
    public virtual Item Item { get; set; } // Navigation property
    public virtual ICollection<Bid> Bids { get; set; } // Collection of Bids
}