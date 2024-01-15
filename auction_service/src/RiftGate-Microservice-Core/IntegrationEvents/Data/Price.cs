using ProtoBuf;

namespace IntegrationEvents.Data;

[ProtoContract]
public record Price
{
    [ProtoMember(1)]
    public decimal Value { get; private set; }
}