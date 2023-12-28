using Infrastructure.Configuration;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.UtilityServices._Interfaces;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Infrastructure.Persistence.MongoDB;

public static class MongoDbRegistration
{
    public static IServiceCollection AddMongoDbServices(this IServiceCollection services, ICustomConfiguration config)
    {

        var connectionString = config.MongoConnectionString;
        var environment = config.Environment;
        var serviceName = config.ServiceName;
        var databaseName = $"{serviceName}_{environment}";

        services.AddSingleton<IMongoClient>(sp =>
        {
            var settings = MongoClientSettings.FromConnectionString(connectionString);
            settings.ServerApi = new ServerApi(ServerApiVersion.V1);

            var client = new MongoClient(settings);

            // Ping test
            try 
            {
                var result = client.GetDatabase("admin").RunCommand<BsonDocument>(new BsonDocument("ping", 1));
                Console.WriteLine("\n###################################");
                Console.WriteLine("\nYou successfully connected to MongoDB! \n");
            } 
            catch (Exception ex) 
            {
                Console.WriteLine($"\n{ex}");
            }

            return client;
        });

        services.AddSingleton<IMongoDbManager, MongoDbManager>(sp =>
        {
            var client = sp.GetRequiredService<IMongoClient>();
            var databases = client.ListDatabaseNames().ToEnumerable().ToDictionary(name => name, name => name);

            // Print the database names to the console
            Console.WriteLine("You have following collections: \n");
            foreach (var dbName in databases.Keys)
            {
                Console.WriteLine($"* {dbName}");
            }

            //Console.WriteLine(environment);

            //Only drop databases if in Development environment
            if (environment.Equals("Development"))
            {
                foreach (var dbName in databases.Keys)
                {

                    //Console.WriteLine(dbName);

                    if(dbName.Equals(databaseName)) {

                        // Drop the entire database
                        client.DropDatabase(dbName);

                        Console.WriteLine($"\nDatabase: {dbName} are now cleared due to ENV: Development...");
                    }
                    
                }
            }

            return new MongoDbManager(client, databaseName, databases);
        });

        return services;
    }
}