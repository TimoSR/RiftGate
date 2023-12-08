using Infrastructure.Utilities._Interfaces;
using ProtoBuf;

namespace Infrastructure.Utilities.Serializers;

public class ProtobufSerializer : IProtobufSerializer
{
    
    // It only works with models that uses Protobuf-Net Attributes!!!!
    
    public string Serialize<TData>(TData content)
    {
        try
        {
            string encodedString = ConvertToProtobuf(content);
        
            return encodedString;
        }
        catch (ProtoException ex)
        {
            // Handle protobuf-specific errors.
            throw new InvalidOperationException($"Failed to serialize content of type {typeof(TData).Name} due to protobuf error.", ex);
        }
        catch (Exception ex) // This will catch any other exception
        {
            // Handle generic errors.
            throw new InvalidOperationException($"An unexpected error occurred while serializing content of type {typeof(TData).Name}.", ex);
        }
    }
    
    public TModel Deserialize<TModel>(string content)
    {
        try
        {
                
            var payload = ConvertToModel<TModel>(content);
                
            return payload;
        }
        catch (ProtoException ex)
        {
            // Handle protobuf-specific errors.
            throw new InvalidOperationException($"Failed to deserialize content to type {typeof(TModel).Name} due to protobuf error.", ex);
        }
        catch (Exception ex) // This will catch any other exception
        {
            // Handle generic errors.
            throw new InvalidOperationException($"An unexpected error occurred while deserializing content to type {typeof(TModel).Name}.", ex);
        }
    }

    private string ConvertToProtobuf<TData>(TData message)
    {
        using var ms = new MemoryStream();
        Serializer.Serialize(ms, message);
        byte[] byteArray = ms.ToArray();
        return Convert.ToBase64String(byteArray);
    }
    
    private TModel ConvertToModel<TModel>(string content)
    {
        byte[] byteArray = Convert.FromBase64String(content);
        using var ms = new MemoryStream(byteArray);
        return Serializer.Deserialize<TModel>(ms);
    }
}  