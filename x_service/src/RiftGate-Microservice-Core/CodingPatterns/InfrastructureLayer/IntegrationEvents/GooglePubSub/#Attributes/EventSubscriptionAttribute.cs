namespace _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = true)]
public class EventSubscriptionAttribute : Attribute
{
    public string EventName { get; }

    public EventSubscriptionAttribute(string eventName)
    {
        EventName = eventName;
    }
}