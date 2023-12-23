using System.Reflection;
using CodingPatterns.DomainLayer;

namespace API._DIRegister;

public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && typeof(IDomainService).IsAssignableFrom(t));

        foreach (var type in types)
        {
            var serviceType = type.GetInterfaces().Except(new[] { typeof(IDomainService) }).FirstOrDefault();

            if (serviceType != null)
            {
                services.AddScoped(serviceType, type);
                Console.WriteLine($"Registered domain service: {type.Name} for {serviceType.Name}");
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