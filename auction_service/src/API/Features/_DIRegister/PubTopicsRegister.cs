using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using Google.Api.Gax.ResourceNames;
using Google.Cloud.PubSub.V1;
using Infrastructure.UtilityServices._Interfaces;

namespace API.Features._DIRegister;

public class PubTopicsRegister
{
    private readonly PublisherServiceApiClient _publisherClient;
    private readonly string _projectId;
    private readonly string _serviceName;

    // If the Attribute is not set at the event, it wont be registered.
    
    public PubTopicsRegister(ICustomConfiguration config, PublisherServiceApiClient publisherClient)
    {
        _projectId = config.ProjectId;
        _serviceName = config.ServiceName;
        _publisherClient = publisherClient;
        RegisterTopics();
        ListAllTopicNames();
    }

    private void RegisterTopics()
    {
        // Using reflection to get all types implementing IPubEvent
        var eventTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.GetInterfaces().Contains(typeof(IPublishIntegrationEvent)) && !type.IsInterface)
            .ToList();

        foreach (var eventType in eventTypes)
        {
            var topicAttribute = (TopicNameAttribute)Attribute.GetCustomAttribute(eventType, typeof(TopicNameAttribute));

            if (topicAttribute == null) 
            {
                // Ignore the event if the attribute is not set.
                return;
            }

            var topicId = $"{_serviceName}-{topicAttribute.Name}";

            if(!string.IsNullOrEmpty(topicId))
            {
                InitializeTopic(topicId);
            }
        }
    }
    
    private void InitializeTopic(string topicId)
    {
        var topicName = TopicName.FromProjectTopic(_projectId, topicId);
        try
        {
            _publisherClient.GetTopic(topicName);
        }
        catch (Grpc.Core.RpcException e) when (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
        {
            _publisherClient.CreateTopic(topicName);
        }
    }
    
    private void ListAllTopicNames()
    {
        ProjectName projectName = new ProjectName(_projectId);

        var allTopics = _publisherClient.ListTopics(projectName);

        Console.WriteLine("\nTopics in PubSub:\n");

        foreach (var topic in allTopics)
        {
            Console.WriteLine($"Topic: {topic.TopicName}");
        }
    }
    
    // private void IfDevelopment()
    // {
    //
    //     var serviceName = _serviceName;
    //     
    //     if (_environment == "Development")
    //     {
    //         Console.WriteLine("\nDeleting Topics due to ENV: Development...");
    //
    //         // Get all environment variables
    //         var environmentVariables = _environmentVariables;
    //
    //         foreach (var key in environmentVariables.Keys)
    //         {
    //             // Check if the environment variable starts with 'TOPIC_'
    //             if (key.ToString().StartsWith("TOPIC_"))
    //             {
    //                 // Get the topic name
    //                 var topicName = $"{serviceName}-{environmentVariables[key].ToString()}";
    //             
    //                 // Delete the topic
    //                 var existingTopic = _publisherClient.GetTopic(new TopicName(_projectId, topicName));
    //                 if (existingTopic != null)
    //                 {
    //                     _publisherClient.DeleteTopic(existingTopic.TopicName);
    //                 }
    //             }
    //         }
    //         
    //     }
    // }
    //
    // private void CreateTopics()
    // {
    //     // Get all environment variables
    //     var environmentVariables = _environmentVariables;
    //     var serviceName = _serviceName;
    //     var topics = new List<string>();
    //
    //     Console.WriteLine("\nCreating Topics:");
    //
    //     // Filter environment variables starting with "TOPIC_"
    //     foreach (DictionaryEntry variable in environmentVariables)
    //     {
    //         string key = variable.Key.ToString();
    //         if (key.StartsWith("TOPIC_"))
    //         {
    //             Console.WriteLine($"\n{variable.Key}");
    //             Console.WriteLine($"{variable.Value}");
    //             var topicID = $"{serviceName}-{variable.Value.ToString()}";
    //             topics.Add(topicID);
    //         }
    //     }
    //
    //     // Create topics if they don't exist
    //     foreach (var topicId in topics)
    //     {
    //         var topicName = TopicName.FromProjectTopic(_projectId, topicId);
    //         try 
    //         {
    //             _publisherClient.GetTopic(topicName);
    //         } 
    //         catch (Grpc.Core.RpcException e) when (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
    //         {
    //             _publisherClient.CreateTopic(topicName);
    //         }
    //     }
    // }
    //
    // private async Task CreateTopicsAsync()
    // {
    //     // Get all environment variables
    //     var environmentVariables = _environmentVariables;
    //     var serviceName = _serviceName;
    //     var topics = new List<string>();
    //
    //     Console.WriteLine("\nCreating Topics:");
    //
    //     // Filter environment variables starting with "TOPIC_"
    //     foreach (DictionaryEntry variable in environmentVariables)
    //     {
    //         string key = variable.Key.ToString();
    //         if (key.StartsWith("TOPIC_"))
    //         {
    //             Console.WriteLine($"\n{variable.Key}");
    //             Console.WriteLine($"{variable.Value}");
    //             var topicID = $"{serviceName}-{variable.Value.ToString()}";
    //             topics.Add(topicID);
    //         }
    //     }
    //
    //     // Create topics if they don't exist
    //     var tasks = topics.Select(async topicId =>
    //     {
    //         var topicName = TopicName.FromProjectTopic(_projectId, topicId);
    //         try 
    //         {
    //             await _publisherClient.GetTopicAsync(topicName);
    //         } 
    //         catch (Grpc.Core.RpcException e) when (e.Status.StatusCode == Grpc.Core.StatusCode.NotFound)
    //         {
    //             await _publisherClient.CreateTopicAsync(topicName);
    //         }
    //     });
    //
    //     await Task.WhenAll(tasks);
    // }
}