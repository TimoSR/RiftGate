namespace CodingPatterns.InfrastructureLayer.IntegrationEvents.GooglePubSub._Attributes;

[AttributeUsage(AttributeTargets.Struct)]
public class TopicNameAttribute : Attribute
{
    public string Name { get; }

    public TopicNameAttribute(string name)
    {
        Name = name;
    }
}
