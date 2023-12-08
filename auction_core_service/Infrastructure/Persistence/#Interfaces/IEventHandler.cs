using _SharedKernel.Patterns.RegistrationHooks.Events._Interfaces;

namespace Infrastructure.Persistence._Interfaces;

public interface IEventHandler
{
    Task PublishJsonEventAsync<TEvent>(TEvent eventMessage) where TEvent : IPubEvent;
    Task PublishProtobufEventAsync<TEvent>(TEvent eventMessage) where TEvent : IPubEvent;
    TEvent? ProcessReceivedEvent<TEvent>(string receivedEvent) where TEvent : class;
}