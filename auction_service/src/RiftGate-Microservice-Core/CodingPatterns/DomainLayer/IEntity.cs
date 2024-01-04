using MediatR;
using MongoDB.Bson.Serialization.Attributes;

namespace CodingPatterns.DomainLayer;

public interface IEntity
{
    string Id { get; }
}