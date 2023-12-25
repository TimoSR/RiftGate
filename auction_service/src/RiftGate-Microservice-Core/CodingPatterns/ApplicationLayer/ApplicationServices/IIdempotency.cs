namespace CodingPatterns.ApplicationLayer.ApplicationServices;

public interface IIdempotency
{
    Guid RequestId { get; set; }
}