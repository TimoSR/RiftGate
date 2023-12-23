using CodingPatterns.InfrastructureLayer.Utilities;

namespace Infrastructure.UtilityServices;

public interface IJsonSerializer : IFrastructureService
{
    string Serialize<TData>(TData content);
    TModel? Deserialize<TModel>(string content);
}