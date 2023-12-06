using Application.Controllers.GraphQL.GraphQL._Interfaces;

namespace Application._Registration.GraphQL;

public static class GraphQlRegistration
{
    public static IServiceCollection AddGraphQlServices(this IServiceCollection services)
    {
        var queryTypes = GetTypes<IQuery>();
        var mutationTypes = GetTypes<IMutation>();
        var subscriptionTypes = GetTypes<ISubscription>();

        var server = services
            .AddGraphQLServer()
            .AddInMemorySubscriptions();

        // Dynamically register queries
        foreach (var type in queryTypes)
        {
            server.AddQueryType(type);
        }

        // Dynamically register mutations
        foreach (var type in mutationTypes)
        {
            server.AddMutationType(type);
        }

        // Dynamically register subscriptions
        foreach (var type in subscriptionTypes)
        {
            server.AddSubscriptionType(type);
        }

        return services;
    }

    private static IEnumerable<Type> GetTypes<TInterface>() where TInterface : class
    {
        var interfaceType = typeof(TInterface);
        var types = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass && interfaceType.IsAssignableFrom(type));
        return types;
    }
}
