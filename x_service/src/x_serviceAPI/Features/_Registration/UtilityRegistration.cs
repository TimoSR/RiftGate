using System.Reflection;
using CodingPatterns.Infrastructure.Utilities;

namespace x_serviceAPI.Features._Registration;

public static class UtilityRegistration
{
    public static IServiceCollection RegisterUtilityServices(this IServiceCollection services)
    {   
        
        // Fetch all types that are classes and implement the IUtilityTool interface.
        var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces().Contains(typeof(IUtilityService)));

        foreach (var type in types)
        {
            // For each type, find the first interface that it implements.
            var serviceType = type.GetInterfaces().First(); // Assumes each class implements only one interface. Customize as needed.

            // Register the type with the DI container.
            services.AddSingleton(serviceType, type);
        }

        return services;
    }
}