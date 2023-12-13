using Newtonsoft.Json;

namespace CodingPatterns.InfrastructureLayer.GooglePubSub;

public class PubSubEvent
{
    [JsonProperty("message")]
    public MessageDetails Message { get; set; }

    [JsonProperty("subscription")]
    public string Subscription { get; set; }
}

public class MessageDetails
{
    [JsonProperty("attributes")]
    public Attributes Attributes { get; set; }

    [JsonProperty("data")]
    public string Data { get; set; }

    [JsonProperty("messageId")]
    public string MessageId { get; set; }

    [JsonProperty("publishTime")]
    public string PublishTime { get; set; }
}

public class Attributes
{
    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("eventType")]
    public string EventType { get; set; }
}