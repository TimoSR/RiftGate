namespace CodingPatterns.Infrastructure.IntegrationEvents;

public interface IPublishIntegrationEvent : IIntegrationEvent
{
    public string Message { get; }
}