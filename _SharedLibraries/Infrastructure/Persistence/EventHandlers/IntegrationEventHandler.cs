using System.Reflection;
using _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;
using CodingPatterns.InfrastructureLayer.GooglePubSub;
using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using Google.Cloud.PubSub.V1;
using Google.Protobuf;
using Infrastructure.Persistence._Interfaces;
using Infrastructure.UtilityServices._Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Encoding = System.Text.Encoding;

namespace Infrastructure.Persistence.EventHandlers;

public class IntegrationEventHandler : IIntegrationEventHandler
{
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IProtobufSerializer _protobufSerializer;
    private readonly PublisherServiceApiClient _publisherService;
    private readonly ILogger<IntegrationEventHandler> _logger;
    private readonly string _projectId;
    private readonly string _serviceName;

    public IntegrationEventHandler(
        ICustomConfiguration config,
        PublisherServiceApiClient publisherService,
        IJsonSerializer jsonSerializer,
        IProtobufSerializer protobufSerializer,
        ILogger<IntegrationEventHandler> logger
        )
    {
        _projectId = config.ProjectId;
        _serviceName = config.ServiceName;
        _publisherService = publisherService;
        _logger = logger;
        _jsonSerializer = jsonSerializer;
        _protobufSerializer = protobufSerializer;
    }

    public TEvent? ProcessReceivedEvent<TEvent>(string receivedEvent) where TEvent : class, ISubscribeIntegrationEvent
    {   
        var pubSubEvent = JsonConvert.DeserializeObject<PubSubEvent>(receivedEvent);
        if (pubSubEvent == null)
        {
            _logger.LogWarning("Received event is null or not in expected format.");
            return null;
        }

        byte[] data = Convert.FromBase64String(pubSubEvent.Message.Data);
        string decodedString = Encoding.UTF8.GetString(data);
        
        TEvent? deserializedEvent = TryDeserialize(decodedString, _protobufSerializer.Deserialize<TEvent>);
        if (deserializedEvent != null)
        {
            LogEventProcessed(pubSubEvent);
            return deserializedEvent;
        }
        
        deserializedEvent = TryDeserialize(decodedString, JsonConvert.DeserializeObject<TEvent>);
        if (deserializedEvent != null)
        {
            LogEventProcessed(pubSubEvent);
            return deserializedEvent;
        }
    
        _logger.LogError("Both JSON and Protobuf Deserialization failed for event data: {EventData}", decodedString);
        return null;
    }
    
    private TEvent? TryDeserialize<TEvent>(string data, Func<string, TEvent?> deserializeFunc) where TEvent : class, ISubscribeIntegrationEvent
    {
        try
        {
            return deserializeFunc(data);
        }
        catch
        {
            // Not logging here as we only want to log if both deserialization methods fail
            return null;
        }
    }

    private void LogEventProcessed(PubSubEvent pubSubEvent)
    {
        _logger.LogInformation("Event Processed: Description: {Description}, Event Type: {EventType}, Message ID: {MessageID}, Publish Time: {PublishTime}",
            pubSubEvent.Message.Attributes.Description,
            pubSubEvent.Message.Attributes.EventType,
            pubSubEvent.Message.MessageId,
            pubSubEvent.Message.PublishTime);
    }
    
    public async Task PublishJsonEventAsync<TEvent>(TEvent eventMessage) where TEvent : IPublishIntegrationEvent
    {
        var eventType = typeof(TEvent);
        var serializedMessage = _jsonSerializer.Serialize(eventMessage);
        string topicId = GenerateTopicID(eventType);
        await PublishMessageAsync(eventMessage, topicId, eventType.Name, serializedMessage);
    }

    public async Task PublishProtobufEventAsync<TEvent>(TEvent eventMessage) where TEvent : IPublishIntegrationEvent
    {
        var eventType = typeof(TEvent);
        var serializedMessage = _protobufSerializer.Serialize(eventMessage);
        string topicId = GenerateTopicID(eventType);
        await PublishMessageAsync(eventMessage, topicId, eventType.Name, serializedMessage);
    }

    private string GenerateTopicID(Type eventType)
    {
        var topicAttribute = eventType.GetCustomAttribute<TopicNameAttribute>();

        // If the class doesn't have the attribute, fall back to using the class name
        var eventName = topicAttribute?.Name ?? eventType.Name;

        return $"{_serviceName}-{eventName}";
    }

    private async Task PublishMessageAsync<TEvent>(TEvent @event, string topicId, string eventType, string formattedMessage) where TEvent : IPublishIntegrationEvent
    {
        var topicName = TopicName.FromProjectTopic(_projectId, topicId);
        
        try
        {
            PubsubMessage pubsubMessage = new PubsubMessage
            {
                Data = ByteString.CopyFromUtf8(formattedMessage),
                Attributes =
                {
                    { "eventType", eventType },
                    { "description", $"{@event.Message}" }
                }
            };

            await _publisherService.PublishAsync(topicName, new[] { pubsubMessage });

            _logger.LogInformation("Message published successfully to {TopicName} with event type {EventType}", topicName, eventType);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred while publishing message to {TopicName} with event type {EventType}", topicName, eventType);
            throw; // Re-throw the exception to maintain the original stack trace
        }
    }
}