using System.Reflection;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Infrastructure.Swagger;

public static class SwaggerRegistration
{
    public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            var documentConfigs = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(ControllerBase)))
                .SelectMany(t =>
                    t.GetCustomAttributes<SwaggerDocAttribute>().Select(d => new { DocName = d.DocName, ControllerType = t })
                        .Concat(
                            t.GetCustomAttributes<ApiVersionAttribute>().Select(v => new { DocName = v.Version, ControllerType = t })
                        )
                )
                .Select(item => new
                {
                    DocName = item.DocName,
                    ApiVersion = item.ControllerType.GetCustomAttribute<ApiVersionAttribute>()?.Version
                })
                .Distinct();

            Dictionary<string, List<string>> docNameToVersionMap = new Dictionary<string, List<string>>();

            foreach (var config in documentConfigs)
            {
                var docKey = $"{config.DocName} {config.ApiVersion}";
                c.SwaggerDoc(docKey, new OpenApiInfo { Title = config.DocName, Version = config.ApiVersion });
                if (!docNameToVersionMap.ContainsKey(config.DocName))
                {
                    docNameToVersionMap[config.DocName] = new List<string>();
                }
                docNameToVersionMap[config.DocName].Add(config.ApiVersion);
            }

            c.DocInclusionPredicate((docName, apiDesc) =>
            {
                var controllerActionDescriptor = apiDesc.ActionDescriptor as ControllerActionDescriptor;
                if (controllerActionDescriptor != null)
                {
                    var swaggerDocAttr = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<SwaggerDocAttribute>();
                    var apiVersionAttr = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<ApiVersionAttribute>();

                    if (swaggerDocAttr != null && apiVersionAttr != null)
                    {
                        var expectedDocName = $"{swaggerDocAttr.DocName} {apiVersionAttr.Version}";
                        return docName.Equals(expectedDocName, StringComparison.OrdinalIgnoreCase);
                    }
                }
                return false;
            });
        
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme.",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new List<string>()
                }
            });
        });
        
        return services;
    }
    
    public static void GenerateSwaggerDocs(this WebApplication app)
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            var documentConfigs = new List<DocumentConfig>();
            var controllerTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.IsSubclassOf(typeof(ControllerBase)));

            foreach (var controllerType in controllerTypes)
            {
                var swaggerDocAttribute = controllerType.GetCustomAttribute<SwaggerDocAttribute>();
                var apiVersionAttribute = controllerType.GetCustomAttribute<ApiVersionAttribute>();

                if (swaggerDocAttribute != null && apiVersionAttribute != null)
                {
                    var apiVersion = apiVersionAttribute.Version;
                    documentConfigs.Add(new DocumentConfig
                    {
                        DocName = swaggerDocAttribute.DocName,
                        ApiVersion = apiVersion
                    });
                }
            }

            // Sort by DocName, then by ApiVersion
            var sortedDocumentConfigs = documentConfigs
                .OrderBy(dc => dc.DocName)
                .ThenBy(dc => dc.ApiVersion)
                .ToList();

            foreach (var config in sortedDocumentConfigs)
            {
                var endpointName = $"{config.DocName} {config.ApiVersion}";
                c.SwaggerEndpoint($"/swagger/{config.DocName} {config.ApiVersion}/swagger.json", endpointName);
            }
        });
    }
    
    public class DocumentConfig
    {
        public string DocName { get; set; }
        public string ApiVersion { get; set; }
    }
}

