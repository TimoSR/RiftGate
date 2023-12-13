using System.Reflection;
using CodingPatterns.ApplicationLayer.ApplicationServices;

namespace API.Features._DIRegister;

public static class AppServiceRegister
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {   
        
        // Fetch all types that are classes and implement the IUtilityTool interface.
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(typeof(IAppService)));

        foreach (var type in types)
        {
            // For each type, find the first interface that it implements.
            var serviceType = type.GetInterfaces().First(); // Assumes each class implements only one interface. Customize as needed.

            // Register the type with the DI container.
            services.AddScoped(serviceType, type);
        }

        return services;
    }
}