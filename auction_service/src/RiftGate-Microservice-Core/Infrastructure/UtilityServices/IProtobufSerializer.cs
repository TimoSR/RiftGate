using CodingPatterns.InfrastructureLayer.Utilities;

namespace Infrastructure.UtilityServices;

public interface IProtobufSerializer : IInfrastructureService
{
    string Serialize<TData>(TData content);
    TModel Deserialize<TModel>(string content);
}