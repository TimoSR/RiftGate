namespace Application.DTO.AuctionService;

public class AuctionDto
{
    public int AuctionId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal StartingBid { get; set; }
    public decimal CurrentBid { get; set; }
    public string ItemName { get; set; } // Assuming the name of the item is needed
    public string ItemDescription { get; set; } // Assuming a description of the item is also needed
    // Include any additional properties that the UI would display about an auction
}

public class BidDto
{
    public int BidId { get; set; }
    public int AuctionId { get; set; }
    public int BidderId { get; set; }
    public decimal Amount { get; set; }
    public DateTime BidTime { get; set; }
    // Include any other properties that would be relevant to display about a bid
}
