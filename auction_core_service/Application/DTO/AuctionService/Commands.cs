namespace Application.DTO.AuctionService;

public class CreateAuctionCommand
{
    public int ItemId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public decimal StartingBid { get; set; }
    // Add any other properties that are required to create an auction
}

public class PlaceBidCommand
{
    public int AuctionId { get; set; }
    public int UserId { get; set; }
    public decimal BidAmount { get; set; }
    // Additional properties for placing a bid, like timestamp if needed
}

public class CancelAuctionCommand
{
    public int AuctionId { get; set; }
    // Additional properties if needed, such as a reason for cancellation
}
