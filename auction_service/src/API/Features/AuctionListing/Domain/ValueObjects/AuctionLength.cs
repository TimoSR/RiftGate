namespace API.Features.AuctionListing.Domain.ValueObjects;

public record AuctionLength
{
    public int Value { get; }
    private readonly int[] _allowedValues = { 12, 24, 48 };

    public AuctionLength(int value)
    {
        if (!_allowedValues.Contains(value))
        {
            throw new ArgumentException(
                $"Invalid auction length. Allowed values are: {string.Join(", ", _allowedValues)}.");
        }

        Value = value;
    }
}