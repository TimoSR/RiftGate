using CodingPatterns.ApplicationLayer.ServiceResultPattern._Enums;

namespace CodingPatterns.ApplicationLayer.ServiceResultPattern;

public class ServiceResult
{
    public bool IsSuccess { get; protected init; }
    public List<string> Messages { get; private init; } = new(); // Assuming messages will not be null
    public ServiceErrorType ErrorType { get; protected init; } = ServiceErrorType.None;

    // Factory method for Success
    public static ServiceResult Success(string? message = null)
    {
        var result = new ServiceResult { IsSuccess = true };
        if (message != null)
        {
            result.Messages.Add(message);
        }
        return result;
    }

    // Factory method for Failure with multiple messages
    public static ServiceResult Failure(IEnumerable<string> messages, ServiceErrorType errorType = ServiceErrorType.InternalError)
    {
        return new ServiceResult { IsSuccess = false, Messages = new List<string>(messages), ErrorType = errorType };
    }

    // Factory method for Failure with a single message
    public static ServiceResult Failure(string message, ServiceErrorType errorType = ServiceErrorType.InternalError)
    {
        return new ServiceResult { IsSuccess = false, Messages = new List<string> { message }, ErrorType = errorType };
    }
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; private set; }

    private ServiceResult(T? data, string? message, bool isSuccess, ServiceErrorType errorType = ServiceErrorType.None) : base()
    {
        Data = data;
        IsSuccess = isSuccess;
        ErrorType = errorType;
        if (message != null)
        {
            Messages.Add(message);
        }
    }

    public static ServiceResult<T> Success(T? data, string? message = null)
    {
        return new ServiceResult<T>(data, message, true);
    }

    public new static ServiceResult<T> Failure(IEnumerable<string> messages, ServiceErrorType errorType = ServiceErrorType.InternalError)
    {
        var result = new ServiceResult<T>(default, null, false, errorType);
        result.Messages.AddRange(messages);
        return result;
    }

    public new static ServiceResult<T> Failure(string message, ServiceErrorType errorType = ServiceErrorType.InternalError)
    {
        return Failure(new List<string> { message }, errorType);
    }
}
