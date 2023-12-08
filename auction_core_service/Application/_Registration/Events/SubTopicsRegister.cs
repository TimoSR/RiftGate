using System.Collections;
using System.Reflection;
using _SharedKernel.Patterns.RegistrationHooks.Events._Attributes;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.PubSub.V1;
using Microsoft.AspNetCore.Mvc;
using IConfiguration = Infrastructure.Utilities._Interfaces.IConfiguration;

namespace Application._Registration.Events;

public class SubTopicsRegister
{
    private readonly SubscriberServiceApiClient _subscriberService;
    private readonly string _projectId;
    private readonly string _serviceName;
    private readonly string _environment;
    private readonly string _hostUrl;
    private readonly IDictionary _environmentVariables;

    public SubTopicsRegister(IConfiguration config, SubscriberServiceApiClient subscriberService)
    {
        _projectId = config.ProjectId;
        _serviceName = config.ServiceName;
        _environment = config.Environment;
        _environmentVariables = config.EnvironmentVariables;
        _subscriberService = subscriberService;
        _hostUrl = config.HostUrl;
        //IfDevelopment();
        RegisterPullSubscriptions();
        RegisterPushSubscriptions();
        ListAllSubscriptions();        
    }

    private void RegisterPullSubscriptions() 
    {
        // Get all environment variables
        var envVars = _environmentVariables;

        Console.WriteLine("\nRegistering Pull Subscriptions:");

        // Filter environment variables starting with "PULL_SUBSCRIBE_"
        foreach (DictionaryEntry variable in envVars)
        {
            string key = variable.Key.ToString();
            
            if (key.StartsWith("PULL_SUBSCRIBE_"))
            {
                Console.WriteLine($"\n{variable.Key}");
                Console.WriteLine($"{variable.Value}");

                var topicValue = variable.Value.ToString();
                var subscriptionId = $"{topicValue}-{_serviceName}";

                RegisterPullSubscription(subscriptionId, topicValue);
            }
        }
    }  

    private void RegisterPullSubscription(string subscriptionId, string topicValue)
    {
        // Code to create a pull subscription
        // No need to specify a pushEndpoint, just bind the subscription to the topic.
        // If using the Google Cloud Pub/Sub client library, this could be done with:

        var publisher = _subscriberService;
        var topicName = new TopicName(_projectId, topicValue);
        var subscriptionName = new SubscriptionName(_projectId, subscriptionId);

        try
        {
            var subscription = publisher.CreateSubscription(subscriptionName, topicName, pushConfig: null, ackDeadlineSeconds: 60);
            Console.WriteLine($"Pull Subscription {subscriptionName} created.");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Failed to create pull subscription: {ex.Message}");
        }
    }

    // I have the base URL from the HostUrl property from the config
    // I know which event i will subscribe to based on the handler i created in the controller
    // I know what the endpoint will for the controller will be called in the controller
    // Using a combination of events and attributes I should be able to define the full endpoint to register
    
    public void RegisterPushSubscriptions()
    {
        Console.WriteLine("\nRegistering Push Subscriptions:");

        var controllers = Assembly.GetExecutingAssembly().ExportedTypes
            .Where(t => t.IsSubclassOf(typeof(ControllerBase)))
            .ToArray();

        foreach (var controller in controllers)
        {
            var methods = controller.GetMethods()
                .Where(m => m.GetCustomAttributes(typeof(EventSubscriptionAttribute), false).Length > 0)
                .ToArray();

            foreach (var method in methods)
            {
                var attribute = (EventSubscriptionAttribute)method.GetCustomAttribute(typeof(EventSubscriptionAttribute));
                var eventName = attribute.EventName;

                var endpointUrl = $"{_hostUrl}/api/{controller.Name.Replace("Controller", "")}/{method.Name}";

                var subscriptionId = $"{eventName}-{_serviceName}";
                
                Console.WriteLine($"{subscriptionId}");
                Console.WriteLine($"{endpointUrl}");

                RegisterPushSubscription(subscriptionId, eventName, endpointUrl);
            }
        }
    }
    
    // private void RegisterPushSubscriptions()
    // {
    //     // Get all environment variables
    //     var envVars = _environmentVariables;
    //
    //     Console.WriteLine("\nRegistering Push Subscriptions:");
    //
    //     // Filter environment variables starting with "PUSH_SUBSCRIBE_"
    //     foreach (DictionaryEntry variable in envVars)
    //     {
    //         string key = variable.Key.ToString();
    //
    //         if (key.StartsWith("PUSH_SUBSCRIBE_"))
    //         {
    //             Console.WriteLine($"\n{variable.Key}");
    //             Console.WriteLine($"{variable.Value}");
    //
    //             var keyValue = variable.Key.ToString();
    //             var topicValue = variable.Value.ToString();
    //             var subscriptionId = $"{topicValue}-{_serviceName}";
    //
    //             // Generating the corresponding endpoint environment variable name
    //             var endpointKey = key.Replace("PUSH_SUBSCRIBE_", "PUSH_ENDPOINT_");
    //
    //             // If the topic name is PRODUCT_UPDATES, the corresponding endpoint environment variable would be PUSH_ENDPOINT_PRODUCT_UPDATES.
    //             var pushEndpoint = envVars[endpointKey]?.ToString();
    //
    //             if (string.IsNullOrEmpty(pushEndpoint))
    //             {
    //                 Console.WriteLine($"Warning: Push endpoint for {keyValue} is not defined.");
    //                 continue;
    //             }
    //
    //             RegisterPushSubscription(subscriptionId, topicValue, pushEndpoint);
    //         }
    //     }
    // }   

    private void RegisterPushSubscription(string subscriptionId, string topicValue, string pushEndpoint)
    {
        SubscriptionName subscriptionName = new SubscriptionName(_projectId, subscriptionId);
        TopicName topicName = new TopicName(_projectId, topicValue);
        PushConfig pushConfig = new PushConfig() { PushEndpoint = pushEndpoint };

        try
        {
            _subscriberService.CreateSubscription(subscriptionName, topicName, pushConfig, ackDeadlineSeconds: 60);
            Console.WriteLine($"\nPush Subscription {subscriptionId} has been created for topic {topicValue}.");
        }
        catch(Exception ex)
        {
            Console.WriteLine($"Failed to create pull subscription: {ex.Message}");
        }
    }

    private void ListAllSubscriptions()
    {
        ProjectName projectName = new ProjectName(_projectId);

        var allSubscriptions = _subscriberService.ListSubscriptions(projectName);

        Console.WriteLine("\nSubscriptions in PubSub:\n");

        foreach (var subscription in allSubscriptions)
        {
            Console.WriteLine($"Subscription: {subscription.SubscriptionName}");
        }
    }
    
    private async void RegisterSubscriptionsAsync()
    {
        // Get all environment variables
        var environmentVariables = _environmentVariables;
        var serviceName = _serviceName;

        Console.WriteLine("\nRegistering Subscriptions:");

        // Filter environment variables starting with "SUBSCRIBE_"
        foreach (DictionaryEntry variable in environmentVariables)
        {
            string key = variable.Key.ToString();
            if (key.StartsWith("SUBSCRIBE_"))
            {
                Console.WriteLine($"\n{variable.Key}");
                Console.WriteLine($"{variable.Value}");
                var topicValue = variable.Value.ToString();
                var subscriptionId = $"{topicValue}-{serviceName}";
                await RegisterSubscriptionAsync(subscriptionId, topicValue);
            }
        }
    }

    private async Task RegisterSubscriptionAsync(string subscriptionId, string topicValue)
    {
        SubscriptionName subscriptionName = new SubscriptionName(_projectId, subscriptionId);
        TopicName topicName = new TopicName(_projectId, topicValue);

        try
        {
            // Check if the subscription already exists
            var existingSubscription = await _subscriberService.GetSubscriptionAsync(subscriptionName);
            Console.WriteLine($"Subscription {subscriptionId} already exists.");
        }
        catch (Grpc.Core.RpcException e) when (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            // If the subscription does not exist, create a new one
            await _subscriberService.CreateSubscriptionAsync(subscriptionName, topicName, pushConfig: null, ackDeadlineSeconds: 60);
            Console.WriteLine($"\nSubscription {subscriptionId} has been created for topic {topicValue}.");
        }
    }
}