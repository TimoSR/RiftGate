namespace CodingPatterns.ApplicationLayer.ApplicationServices;

public interface IIdempotency
{
    Guid? RequestID { get; set; }
}