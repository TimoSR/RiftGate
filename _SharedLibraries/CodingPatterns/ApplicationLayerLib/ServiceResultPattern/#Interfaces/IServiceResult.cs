namespace ApplicationLayerLib.ServiceResultPattern._Interfaces;

public interface IServiceResult
{
    bool IsSuccess { get; }
    string? Message { get; }
}