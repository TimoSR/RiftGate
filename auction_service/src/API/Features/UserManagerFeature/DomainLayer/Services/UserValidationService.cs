using _SharedKernel.Patterns.ResultPattern;
using API.Features.UserManagerFeature.DomainLayer.Messages;
using API.Features.UserManagerFeature.DomainLayer.Repositories;
using API.Features.UserManagerFeature.DomainLayer.Services._Interfaces;
using API.Features.UserManagerFeature.DomainLayer.Utilities.Email;
using API.Features.UserManagerFeature.DomainLayer.Utilities.Password;
using CodingPatterns.DomainLayer;

namespace API.Features.UserManagerFeature.DomainLayer.Services;

public class UserValidationService : IUserValidationService, IDomainService
{
    private readonly EmailValidator _emailValidator;
    private readonly PasswordValidator _passwordValidator;
    private readonly IUserRepository _userRepository;

    public UserValidationService(IUserRepository userRepository)
    {
        _userRepository = userRepository;
        _emailValidator = new EmailValidator();
        _passwordValidator = new PasswordValidator();
    }

    public ValidationResult ValidateNewUser(string email, string password)
    {
        var validationResult = new ValidationResult();

        if (!_emailValidator.IsValid(email))
        {
            validationResult.AddError(UserRegMsg.InvalidEmail);
        }

        if (!_passwordValidator.IsValid(password))
        {
            validationResult.AddError(UserRegMsg.InvalidPassword);
        }

        // If there are no errors, validationResult.IsSuccess will be true.
        // If there are errors, validationResult.IsSuccess will be false and Messages will contain the errors.
        return validationResult;
    }
}