namespace Infrastructure.Swagger;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class ApiVersionAttribute : Attribute
{
    public string Version { get; }

    public ApiVersionAttribute(string version)
    {
        Version = version;
    }
}
