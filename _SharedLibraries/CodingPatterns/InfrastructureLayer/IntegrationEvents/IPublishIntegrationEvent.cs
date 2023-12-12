namespace CodingPatterns.InfrastructureLayer.IntegrationEvents;

public interface IPublishIntegrationEvent : IIntegrationEvent
{
    public string Message { get; }
}