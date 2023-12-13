namespace _SharedKernel.Patterns.ResultPattern;

public class ValidationResult
{
    public bool IsSuccess { get; private set; }
    public List<string> Messages { get; private set; } = new List<string>();

    // Constructor to set the default success state.
    public ValidationResult(bool isSuccess = true)
    {
        IsSuccess = isSuccess;
    }

    // Method to add an error message.
    public void AddError(string message)
    {
        // When the first error is added, set IsSuccess to false.
        if (IsSuccess && Messages.Count == 0)
        {
            IsSuccess = false;
        }

        Messages.Add(message);
    }
}