using System.Reflection;
using Infrastructure.Persistence._Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure._Registration.Repositories;

public static class RepositoryRegistration
{
    public static IServiceCollection AddApplicationRepositories(this IServiceCollection services)
    {
        var types = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IRepository<>)))
            .ToList();
    
        if (!types.Any())
        {
            throw new InvalidOperationException("No implementation for IRepository found.");
        }
        
        foreach (var type in types)
        {
            var serviceType = type.GetInterfaces().FirstOrDefault(i => i.Name == "I" + type.Name);
            
            services.AddScoped(serviceType, type);
        }
    
        return services;
    }
}