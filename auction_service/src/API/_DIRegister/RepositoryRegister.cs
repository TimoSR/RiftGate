using System.Reflection;
using CodingPatterns.DomainLayer;
using CodingPatterns.InfrastructureLayer;

namespace API._DIRegister;

public static class RepositoryRegister
{
    public static IServiceCollection AddApplicationRepositories(this IServiceCollection services)
    {
        // Registering IRepository<>
        RegisterRepositories(services);

        // Registering ICachedRepository implementations
        RegisterCachedRepositories(services);

        return services;
    }
    
    
    private static void RegisterRepositories(IServiceCollection services)
    {
        // Registering IRepositories
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
            Console.WriteLine($"Registered repository: {type.Name} for {serviceType.Name}");
        }
    }
    
    private static void RegisterCachedRepositories(IServiceCollection services)
    {
        var cachedRepoTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.GetInterfaces()
                .Any(i => typeof(ICachedRepository).IsAssignableFrom(i) && i != typeof(ICachedRepository)))
            .ToList();

        foreach (var type in cachedRepoTypes)
        {
            var serviceType = type.GetInterfaces().FirstOrDefault(i => typeof(ICachedRepository).IsAssignableFrom(i) && i != typeof(ICachedRepository));
            if (serviceType != null)
            {
                services.AddScoped(serviceType, type);
                Console.WriteLine($"Registered cached repository: {type.Name} for {serviceType.Name}");
            }
        }
    }
}