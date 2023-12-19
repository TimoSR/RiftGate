using API.Features.UserManagerFeature.DomainLayer.Enums;
using CodingPatterns.DomainLayer;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace API.Features.UserManagerFeature.DomainLayer.Entities;

public class User : Entity, IAggregateRoot
{
    public const int MinPasswordLength = 6;
    
    //public override string Id { get; set; }

    private string _email;
    [BsonElement("Email")]
    public string Email 
    { 
        get => _email; 
        set => _email = value?.ToLowerInvariant(); 
    }

    [BsonElement("UserName")]
    public string UserName { get; set; }

    [BsonElement("Status")] 
    public string Status { get; set; } = UserStatus.Pending.ToString();
    
    [BsonElement("LastModified")] 
    public DateTime LastModified { get; set; }
}