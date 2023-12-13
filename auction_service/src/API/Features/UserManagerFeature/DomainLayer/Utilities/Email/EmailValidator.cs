using System.Text.RegularExpressions;

namespace API.Features.UserManagerFeature.DomainLayer.Utilities.Email;

public class EmailValidator
{
    
    private readonly Regex _usernameRegex = new Regex("^[a-zA-Z0-9._-]+$", RegexOptions.Compiled);
    
    // The '@' symbol before the string denotes a verbatim string literal in C#.
    // In a verbatim string, backslashes are treated as literal characters and not as escape characters.
    // This is particularly useful when writing regular expressions, which often contain many backslashes.
    // Without '@', you would need to escape each backslash, making the regex less readable.
    private readonly List<Regex> _popularEmailRegexes = new List<Regex>
    {
        new Regex(@"gmail\.com$", RegexOptions.Compiled),
        new Regex(@"yahoo\.com$", RegexOptions.Compiled),
        new Regex(@"hotmail\.com$", RegexOptions.Compiled),
        new Regex(@"outlook\.com$", RegexOptions.Compiled),
        new Regex(@"msn\.com$", RegexOptions.Compiled),
        new Regex(@"live\.com$", RegexOptions.Compiled),
        new Regex(@"aol\.com$", RegexOptions.Compiled),
        new Regex(@"protonmail\.com$", RegexOptions.Compiled),
        new Regex(@"zoho\.com$", RegexOptions.Compiled),
        new Regex(@"icloud\.com$", RegexOptions.Compiled),
        new Regex(@"mail\.ru$", RegexOptions.Compiled),
        new Regex(@"yandex\.ru$", RegexOptions.Compiled),
        new Regex(@"inbox\.com$", RegexOptions.Compiled),
        new Regex(@"gmx\.com$", RegexOptions.Compiled),
        new Regex(@"fastmail\.com$", RegexOptions.Compiled),
        new Regex(@"hushmail\.com$", RegexOptions.Compiled),
        new Regex(@"lycos\.com$", RegexOptions.Compiled),
        new Regex(@"rediffmail\.com$", RegexOptions.Compiled),
        new Regex(@"sina\.com$", RegexOptions.Compiled),
        new Regex(@"qq\.com$", RegexOptions.Compiled),
    };
    
    public bool IsValid(string email)
    {
        if (string.IsNullOrEmpty(email))
        {
            return false;
        }
        
        // Split email into username and domain parts
        var parts = email.Split('@');

        if (parts.Length != 2)
        {
            return false;
        }

        // Validate username part
        if (!_usernameRegex.IsMatch(parts[0]))
        {
            return false;
        }

        // Validate domain part
        if (!_popularEmailRegexes.Any(regex => regex.IsMatch(parts[1])))
        {
            return false;
        }

        return true;
    }
}