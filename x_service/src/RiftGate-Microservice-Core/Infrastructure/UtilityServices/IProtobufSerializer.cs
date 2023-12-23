using CodingPatterns.InfrastructureLayer.Utilities;

namespace Infrastructure.UtilityServices;

public interface IProtobufSerializer : IFrastructureService
{
    string Serialize<TData>(TData content);
    TModel Deserialize<TModel>(string content);
}