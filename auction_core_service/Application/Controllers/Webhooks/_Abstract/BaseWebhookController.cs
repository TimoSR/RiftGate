using Infrastructure.Persistence._Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Application.Controllers.Webhooks._Abstract
{
    public abstract class BaseWebhookController : ControllerBase
    {
        private readonly IEventHandler _eventHandler;

        protected BaseWebhookController(IEventHandler eventHandler)
        {
            _eventHandler = eventHandler;
        }

        protected async Task<TEventData> OnEvent<TEventData>() where TEventData : class
        {
            using var reader = new StreamReader(Request.Body);
            var receivedEventJson = await reader.ReadToEndAsync();
            return _eventHandler.ProcessReceivedEvent<TEventData>(receivedEventJson);
        }
    }
}