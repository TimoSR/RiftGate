using System.Reflection;
using CodingPatterns.ApplicationLayer.ApplicationServices;

namespace API._DIRegister;

public static class QueryHandlerRegister
{
    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        var queryHandlerType = typeof(IQueryHandler<,>);
        var types = Assembly.GetAssembly(queryHandlerType)?.GetTypes();

        if (types == null) return services;
        
        var handlers = types
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == queryHandlerType))
            .ToList();

        foreach (var handler in handlers)
        {
            foreach (var interfaceType in handler.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == queryHandlerType)
                {
                    services.AddScoped(interfaceType, handler);
                }
            }
        }

        return services;
    }
}