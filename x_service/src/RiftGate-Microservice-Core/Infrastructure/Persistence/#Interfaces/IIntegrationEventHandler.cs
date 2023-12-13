using CodingPatterns.InfrastructureLayer.IntegrationEvents;

namespace Infrastructure.Persistence._Interfaces;

public interface IIntegrationEventHandler
{
    Task PublishJsonEventAsync<TEvent>(TEvent eventMessage) where TEvent : IPublishIntegrationEvent;
    Task PublishProtobufEventAsync<TEvent>(TEvent eventMessage) where TEvent : IPublishIntegrationEvent;
    TEvent? ProcessReceivedEvent<TEvent>(string receivedEvent) where TEvent : class, ISubscribeIntegrationEvent;
}