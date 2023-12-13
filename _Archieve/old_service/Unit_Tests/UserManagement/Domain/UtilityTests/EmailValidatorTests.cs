using Domain.UserManagement.Utilities.Email;

namespace Unit_Tests.UtilityTests;

public class EmailValidatorTests
{
    private readonly EmailValidator _emailValidator = new();

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("missingAtSign.com")]
    [InlineData("missingDomain@")]
    [InlineData("double@@sign.com")]
    public void IsValid_InvalidFormat_ReturnsFalse(string email)
    {
        bool result = _emailValidator.IsValid(email);
        Assert.False(result);
    }

    [Theory]
    [InlineData("in_valid@domain.com")]
    [InlineData("with space@domain.com")]
    [InlineData("-startswithdash@domain.com")]
    public void IsValid_InvalidUsername_ReturnsFalse(string email)
    {
        bool result = _emailValidator.IsValid(email);
        Assert.False(result);
    }

    [Theory]
    [InlineData("valid@unpopular.com")]
    [InlineData("valid@notpopular.com")]
    public void IsValid_InvalidDomain_ReturnsFalse(string email)
    {
        bool result = _emailValidator.IsValid(email);
        Assert.False(result);
    }

    [Theory]
    [InlineData("valid@gmail.com")]
    [InlineData("valid@yahoo.com")]
    public void IsValid_ValidEmail_ReturnsTrue(string email)
    {
        bool result = _emailValidator.IsValid(email);
        Assert.True(result);
    }
}