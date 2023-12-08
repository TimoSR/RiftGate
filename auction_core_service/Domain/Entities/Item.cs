namespace Domain.Entities;

public class Item
{
    public int ItemId { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public decimal StartingPrice { get; set; }

    public virtual ICollection<Auction> Auctions { get; set; } // Collection of Auctions
}