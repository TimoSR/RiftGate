namespace _SharedKernel.Patterns._Interfaces;

public interface IServiceResult
{
    bool IsSuccess { get; }
    string? Message { get; }
}