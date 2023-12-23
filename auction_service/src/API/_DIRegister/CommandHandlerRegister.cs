using System.Reflection;
using CodingPatterns.ApplicationLayer.ApplicationServices;

namespace API._DIRegister;

public static class CommandHandlerRegister
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        var commandHandlerType = typeof(ICommandHandler<>);
        var types = Assembly.GetAssembly(commandHandlerType).GetTypes();

        var handlers = types
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == commandHandlerType))
            .ToList();

        foreach (var handler in handlers)
        {
            foreach (var interfaceType in handler.GetInterfaces())
            {
                if (interfaceType.IsGenericType && interfaceType.GetGenericTypeDefinition() == commandHandlerType)
                {
                    services.AddScoped(interfaceType, handler);
                }
            }
        }

        return services;
    } 
}