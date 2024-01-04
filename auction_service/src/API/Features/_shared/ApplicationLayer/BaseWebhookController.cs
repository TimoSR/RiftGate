using CodingPatterns.InfrastructureLayer.IntegrationEvents;
using Infrastructure.Persistence._Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace API.Features._shared.ApplicationLayer
{
    public abstract class BaseWebhookController : ControllerBase
    {
        private readonly IIntegrationEventHandler _integrationEventHandler;

        protected BaseWebhookController(IIntegrationEventHandler integrationEventHandler)
        {
            _integrationEventHandler = integrationEventHandler;
        }

        protected async Task<TEventData> OnEvent<TEventData>() where TEventData : class, ISubscribeEvent
        {
            using var reader = new StreamReader(Request.Body);
            var receivedEventJson = await reader.ReadToEndAsync();
            return _integrationEventHandler.ProcessReceivedEvent<TEventData>(receivedEventJson);
        }
    }
}