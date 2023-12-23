using System.Reflection;
using CodingPatterns.ApplicationLayer.ApplicationServices;

namespace API._DIRegister;

public static class QueryHandlerRegister
{
    public static IServiceCollection AddQueryHandlers(this IServiceCollection services)
    {
        var queryHandlerType = typeof(IQueryHandler<,>);
        var types = Assembly.GetAssembly(queryHandlerType).GetTypes();

        var handlers = types
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == queryHandlerType))
            .ToList();

        foreach (var handler in handlers)
        {
            foreach (var intf in handler.GetInterfaces())
            {
                if (intf.IsGenericType && intf.GetGenericTypeDefinition() == queryHandlerType)
                {
                    services.AddScoped(intf, handler);
                }
            }
        }

        return services;
    }
}