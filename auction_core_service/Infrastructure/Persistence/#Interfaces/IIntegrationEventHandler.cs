using _SharedKernel.Patterns.RegistrationHooks.Events._Interfaces;

namespace Infrastructure.Persistence._Interfaces;

public interface IIntegrationEventHandler
{
    Task PublishJsonEventAsync<TEvent>(TEvent eventMessage) where TEvent : IDomainEvent;
    Task PublishProtobufEventAsync<TEvent>(TEvent eventMessage) where TEvent : IDomainEvent;
    TEvent? ProcessReceivedEvent<TEvent>(string receivedEvent) where TEvent : class;
}