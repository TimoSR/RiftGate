namespace _SharedKernel.Patterns.InfrastructureLayer.IntegrationEvents;

public interface IPublishIntegrationEvent : IIntegrationEvent
{
    public string Message { get; }
}