using ApplicationLayerLib.ServiceResultPattern._Enums;

namespace ApplicationLayerLib.ServiceResultPattern;

public class ServiceResult
{
    public bool IsSuccess { get; protected set; }
    public List<string?> Messages { get; protected set; } = new();
    public ServiceErrorType ErrorType { get; protected set; } = ServiceErrorType.None;

    // Factory methods with ErrorType
    public static ServiceResult Success(string? message = null)
    {
        var result = new ServiceResult { IsSuccess = true };
        if (message != null)
        {
            result.Messages.Add(message);
        }
        return result;
    }

    public static ServiceResult Failure(IEnumerable<string> messages, ServiceErrorType errorType = ServiceErrorType.InternalError)
    {
        return new ServiceResult { IsSuccess = false, Messages = new List<string>(messages), ErrorType = errorType };
    }

    public static ServiceResult Failure(string message, ServiceErrorType errorType = ServiceErrorType.InternalError)
    {
        return Failure(new List<string> { message }, errorType);
    }
}

public class ServiceResult<T> : ServiceResult
{
    public T? Data { get; private set; }

    private ServiceResult(T data, string? message, bool isSuccess, ServiceErrorType errorType = ServiceErrorType.None): base()
    {
        Data = data;
        IsSuccess = isSuccess;
        ErrorType = errorType;
        if (message != null)
        {
            Messages.Add(message);
        }
    }

    public static ServiceResult<T> Success(T data, string? message = null)
    {
        return new ServiceResult<T>(data, message, true);
    }

    public new static ServiceResult<T?> Failure(IEnumerable<string> messages, ServiceErrorType errorType = ServiceErrorType.InternalError)
    {
        var result = new ServiceResult<T?>(default, null, false, errorType);
        result.Messages.AddRange(messages);
        return result;
    }

    public new static ServiceResult<T?> Failure(string message, ServiceErrorType errorType = ServiceErrorType.InternalError)
    {
        return Failure(new List<string> { message }, errorType);
    }
}
