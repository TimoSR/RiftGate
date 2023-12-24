using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Infrastructure.ValidationAttributes;

public class HexStringAttribute : ValidationAttribute
{
    private readonly int _length;

    public HexStringAttribute(int length)
    {
        _length = length;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        if (value is string stringValue)
        {
            var regex = new Regex($"^[0-9a-fA-F]{{{_length}}}$");

            if (regex.IsMatch(stringValue))
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult(GetErrorMessage());
            }
        }

        return new ValidationResult(GetErrorMessage());
    }

    public string GetErrorMessage()
    {
        return $"The field must be a {_length}-digit hexadecimal string.";
    }
}