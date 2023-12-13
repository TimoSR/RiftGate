namespace Infrastructure.Swagger.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public class SwaggerDocAttribute : Attribute
{
    public SwaggerDocAttribute(string docName) => DocName = docName;

    public string DocName { get; }
}