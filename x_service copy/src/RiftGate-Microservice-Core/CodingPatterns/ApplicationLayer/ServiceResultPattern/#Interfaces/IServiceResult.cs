namespace CodingPatterns.ApplicationLayer.ServiceResultPattern._Interfaces;

public interface IServiceResult
{
    bool IsSuccess { get; }
    string? Message { get; }
}