namespace CodingPatterns.InfrastructureLayer.IntegrationEvents;

public interface IPublishEvent : IIntegrationEvent
{
    public string Message { get; }
}