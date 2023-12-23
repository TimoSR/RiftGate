using System.Reflection;

namespace API._DIRegister;

public static class HostedServiceRegister
{
    public static IServiceCollection AddHostedServices(this IServiceCollection services)
    {
        var hostedServiceType = typeof(IHostedService);
        var assembly = Assembly.GetEntryAssembly(); // Adjust the assembly as needed
        var hostedServices = assembly.GetTypes()
            .Where(t => hostedServiceType.IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var serviceType in hostedServices)
        {
            services.AddSingleton(serviceType, serviceProvider => 
            {
                return ActivatorUtilities.CreateInstance(serviceProvider, serviceType);
            });
            Console.WriteLine($"Registered hosted service: {serviceType.Name}");
        }

        return services;
    }
}