namespace _SharedKernel.Patterns.IntegrationEvents.GooglePubSub._Attributes;

[AttributeUsage(AttributeTargets.Class)]
public class TopicNameAttribute : Attribute
{
    public string Name { get; }

    public TopicNameAttribute(string name)
    {
        Name = name;
    }
}
