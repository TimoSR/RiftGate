using Google.Cloud.PubSub.V1;
using Grpc.Core;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.Utilities._Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence.Google_PubSub;

public static class PubSubRegistration {

    public static IServiceCollection AddPublisherClient(this IServiceCollection services, IConfiguration config)
    {

        var projectId = config.ProjectId;

        // Use this if the microservice won't be utlizing scaling to zero. 
        // As it scales and perform better and support more advanced features
        // services.AddSingleton<PublisherClient>(serviceProvider =>
        // {
        //     Console.WriteLine("\nPublisherClient Created!");
        //     // Here you should initialize your PublisherClient with the settings you need.
        //     return PublisherClient.Create(topicName);
        // });
        
        services.AddSingleton<PublisherServiceApiClient>(serviceProvider => {
            //Console.WriteLine("\nPublisherServiceApiClient Created!");     

            var client = PublisherServiceApiClient.Create();

            try {
                // Try to get a non-existent topic.
                var topicName = new TopicName(projectId, "non-existent-topic");
                var response = client.GetTopic(topicName);
            } catch (RpcException e) when (e.Status.StatusCode == StatusCode.NotFound) {
                // If we get a "not found" error, it means we were able to connect to the server.
                Console.WriteLine("\n###################################");
                Console.WriteLine("\nYou successfully connected to PubSub with PublisherApiClient!");
            } catch (Exception e) {
                // If we get any other exception, it might be a connection error.
                Console.WriteLine($"\nFailed to connect to Pub/Sub server: {e.Message}");
            }

            return client;
        });
        
        services.AddScoped<IIntegrationEventHandler, IntegrationEventHandler>();
            
        return services;
    }
     
    public static IServiceCollection AddSubscriberClient(this IServiceCollection services)
    {
        services.AddSingleton<SubscriberServiceApiClient>(serviceProvider =>
        {
            //Console.WriteLine("\nSubscriberServiceApiClient Created!");

            var client = SubscriberServiceApiClient.Create();

            Console.WriteLine("\n###################################");
            Console.WriteLine("\nYou successfully connected to PubSub with SubscriberApiClient!");

            return client;
        });

        return services;
    }

}