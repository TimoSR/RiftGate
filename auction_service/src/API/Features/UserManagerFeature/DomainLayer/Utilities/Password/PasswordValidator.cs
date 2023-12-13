using System.Text.RegularExpressions;

namespace API.Features.UserManagerFeature.DomainLayer.Utilities.Password;

public class PasswordValidator 
{
    // Minimum length required for the password
    private const int MinimumLength = 6;

    // Regular expressions for various types of characters
    private readonly Regex _upperCase = new Regex("[A-Z]");
    private readonly Regex _lowerCase = new Regex("[a-z]");
    private readonly Regex _digits = new Regex("[0-9]");
    private readonly Regex _specialChars = new Regex("[!@#$%^&*(),.?\":{}|<>]");

    public bool IsValid(string password)
    {
        // Check minimum length
        if (string.IsNullOrEmpty(password) || password.Length < MinimumLength)
        {
            return false;
        }
        
        // Check for lower-case letters
        if (!_lowerCase.IsMatch(password))
        {
            return false;
        }

        // Check for upper-case letters
        if (!_upperCase.IsMatch(password))
        {
            return false;
        }

        // Check for digits
        if (!_digits.IsMatch(password))
        {
            return false;
        }

        // Check for special characters
        if (!_specialChars.IsMatch(password))
        {
            return false;
        }

        return true;
    }
}