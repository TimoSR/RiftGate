namespace Domain.Entities;

public class Auction
{
    public int AuctionId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public int ItemId { get; set; }
    public virtual Item Item { get; set; } // Navigation property
    public virtual ICollection<Bid> Bids { get; set; } // Collection of Bids
}