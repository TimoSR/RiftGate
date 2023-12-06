using Infrastructure.Utilities._Interfaces;
using Newtonsoft.Json;

namespace Infrastructure.Utilities.Serializers;

public class JsonSerializer : IJsonSerializer
{
    public string Serialize<TData>(TData content)
    {
        try
        {
            string encodedString = ConvertToJson(content);
            return encodedString;
        }
        catch (JsonException ex)
        {
            // Handle JSON-specific errors.
            // For example, log the error or provide a specific message for JSON errors.
            throw new InvalidOperationException($"Failed to serialize content of type {typeof(TData).Name} due to JSON error.", ex);
        }
        catch (Exception ex) // This will catch any other exception
        {
            // Handle generic errors.
            // You might want to log the error or take some other action.
            throw new InvalidOperationException($"An unexpected error occurred while serializing content of type {typeof(TData).Name}.", ex);
        }
    }
    
    public TModel? Deserialize<TModel>(string content)
    {
        try
        {
            var payload = ConvertToModel<TModel>(content);
            return payload;
        }
        catch (JsonException ex)
        {
            // Handle JSON-specific errors.
            throw new InvalidOperationException($"Failed to deserialize content to type {typeof(TModel).Name} due to JSON error.", ex);
        }
        catch (Exception ex) // This will catch any other exception
        {
            // Handle generic errors.
            throw new InvalidOperationException($"An unexpected error occurred while deserializing content to type {typeof(TModel).Name}.", ex);
        }
    }

    private string ConvertToJson<TData>(TData message)
    {
        return JsonConvert.SerializeObject(message);
    }

    private TModel? ConvertToModel<TModel>(string content)
    {
        return JsonConvert.DeserializeObject<TModel>(content);
    }
} 