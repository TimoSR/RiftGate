using CodingPatterns.InfrastructureLayer.IntegrationEvents;

namespace Infrastructure.Persistence._Interfaces;

public interface IIntegrationEventHandler
{
    Task PublishJsonEventAsync<TEvent>(TEvent eventMessage) where TEvent : IPublishEvent;
    Task PublishProtobufEventAsync<TEvent>(TEvent eventMessage) where TEvent : IPublishEvent;
    TEvent? ProcessReceivedEvent<TEvent>(string receivedEvent) where TEvent : class, ISubscribeEvent;
}