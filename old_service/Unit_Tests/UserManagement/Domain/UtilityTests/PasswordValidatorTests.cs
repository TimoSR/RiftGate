using Domain.UserManagement.Utilities.Password;

namespace Unit_Tests.UtilityTests;

public class PasswordValidatorTests
{
    private readonly PasswordValidator _passwordValidator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void IsValid_NullOrEmpty_ReturnsFalse(string password)
    {
        bool result = _passwordValidator.IsValid(password);
        Assert.False(result);
    }

    [Theory]
    [InlineData("short")]
    [InlineData("tiny")]
    public void IsValid_LengthLessThanMinimum_ReturnsFalse(string password)
    {
        bool result = _passwordValidator.IsValid(password);
        Assert.False(result);
    }

    [Theory]
    [InlineData("nouppercase1!")]
    [InlineData("missingupper1!")]
    public void IsValid_NoUpperCaseLetter_ReturnsFalse(string password)
    {
        bool result = _passwordValidator.IsValid(password);
        Assert.False(result);
    }

    [Theory]
    [InlineData("NODIGITS!")]
    [InlineData("MISSINGNUM!")]
    public void IsValid_NoDigit_ReturnsFalse(string password)
    {
        bool result = _passwordValidator.IsValid(password);
        Assert.False(result);
    }

    [Theory]
    [InlineData("NoSpecial1")]
    [InlineData("MissingChars1")]
    public void IsValid_NoSpecialChar_ReturnsFalse(string password)
    {
        bool result = _passwordValidator.IsValid(password);
        Assert.False(result);
    }

    [Theory]
    [InlineData("Valid1!")]
    [InlineData("AnotherValid1!")]
    public void IsValid_ValidPassword_ReturnsTrue(string password)
    {
        bool result = _passwordValidator.IsValid(password);
        Assert.True(result);
    }
}