using CodingPatterns.InfrastructureLayer.Utilities;

namespace Infrastructure.UtilityServices._Interfaces;

public interface IJsonSerializer : IUtilityService
{
    string Serialize<TData>(TData content);
    TModel? Deserialize<TModel>(string content);
}