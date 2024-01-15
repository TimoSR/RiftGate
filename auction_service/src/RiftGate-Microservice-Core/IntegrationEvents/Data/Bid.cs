using ProtoBuf;

namespace IntegrationEvents.Data;

[ProtoContract]
public class Bid
{
    [ProtoMember(1)]
    public Price BidAmount { get; set; }

    [ProtoMember(2)]
    public string BidderId { get; set; }
    
    [ProtoMember(3)]
    public DateTime TimeStamp { get; set; }
}