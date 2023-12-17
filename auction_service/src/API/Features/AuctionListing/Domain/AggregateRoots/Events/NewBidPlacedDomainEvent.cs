using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.Domain.AggregateRoots.Events;

public class NewBidPlacedDomainEvent : IDomainEvent
{
    public string Message { get; }
    public string BidId { get; }
    public decimal Value { get; }

    public NewBidPlacedDomainEvent(string bidId, decimal value)
    {
        // As Domain Events can be trusted we don't need to validate the data input
        BidId = bidId;
        Value = value;
        Message = $"New bid {bidId} with value {value} was placed!";
    }
}