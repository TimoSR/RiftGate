using CodingPatterns.InfrastructureLayer;
using Infrastructure.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Infrastructure.Persistence.Redis;

public static class RedisRegistration
{
    public static IServiceCollection AddRedisServices(this IServiceCollection services, ICustomConfiguration config)
    {

        var redisConnectionString = config.RedisConnectionString;

        services.AddSingleton<IConnectionMultiplexer>(serviceProvider =>
        {
            var client = ConnectionMultiplexer.Connect(redisConnectionString);

            // Check the connection status
            if (!client.IsConnected)
            {
                Console.WriteLine("Failed to establish a connection to RedisCacheManagerService.");
                throw new InvalidOperationException("Cannot start application without RedisCacheManagerService connection.");
            }

            Console.WriteLine("\n###################################");

            var db = client.GetDatabase();
            var pong = db.Execute("PING");
            
            if (pong.ToString() == "PONG")
            {
                Console.WriteLine("\nYou successfully connected to Redis!");
            }
            else
            {
                throw new InvalidOperationException("Unexpected response from RedisCacheManagerService.");
            }
            
            Console.WriteLine("\n###################################");

            return client;
        });

        services.AddSingleton<ICacheManager, RedisCacheManagerService>();

        return services;
    }
}