using _SharedKernel.Patterns.RegistrationHooks.Utilities;

namespace Infrastructure.Utilities._Interfaces;

public interface IJsonSerializer : IUtilityTool
{
    string Serialize<TData>(TData content);
    TModel? Deserialize<TModel>(string content);
}