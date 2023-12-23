namespace CodingPatterns.InfrastructureLayer;

public class Result<T>
{
    public T Value { get; }
    public bool IsSuccess { get; }
    public string ErrorMessage { get; }

    protected Result(T value, bool isSuccess, string errorMessage)
    {
        Value = value;
        IsSuccess = isSuccess;
        ErrorMessage = errorMessage;
    }

    public static Result<T> Success(T value) => new Result<T>(value, true, string.Empty);
    public static Result<T> Failure(string message) => new Result<T>(default, false, message);
}
