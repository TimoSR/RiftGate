using CodingPatterns.DomainLayer;

namespace API.Features.AuctionListing.ValueObjects;

public record Price : IValueObject
{
    public decimal Amount { get; init; }

    public Price(decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Amount cannot be negative or zero.");

        Amount = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
    }
}
