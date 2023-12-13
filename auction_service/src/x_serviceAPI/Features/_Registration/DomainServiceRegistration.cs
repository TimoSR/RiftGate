using System.Reflection;
using CodingPatterns.DomainLayer;

namespace x_serviceAPI.Features._Registration;

public static class DomainServiceRegistration
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IDomainService).IsAssignableFrom(t));

        foreach (var type in types)
        {
            // Find the first interface that the class implements, excluding the IDomainService interface
            var serviceType = type.GetInterfaces().Except(new[] {typeof(IDomainService)}).FirstOrDefault();

            if (serviceType != null)
            {
                // Register the class with its corresponding interface
                services.AddScoped(serviceType, type);
            }
            else
            {
                // Optionally handle cases where no other interface is implemented
                // For example, register the type directly, log a warning, etc.
            }
        }

        return services;
    }
}

// using System.Reflection;
// using _SharedKernel.Patterns.RegistrationHooks.Services._Interfaces;
// using Microsoft.Extensions.DependencyInjection;
//
// namespace Domain._Shared.Registration;
//
// public static class DomainServiceRegistration
// {
//     public static IServiceCollection AddDomainServices(this IServiceCollection services)
//     {
//         // Fetch all types that are classes, not abstract, and implement the IDomainService interface.
//         var types = Assembly.GetExecutingAssembly().GetTypes()
//             .Where(t => t.IsClass && !t.IsAbstract && typeof(IDomainService).IsAssignableFrom(t));
//
//         foreach (var type in types)
//         {
//             // Register the type directly without using its interface.
//             services.AddScoped(type);
//         }
//
//         return services;
//     }
// }