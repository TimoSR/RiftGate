using ValidationResult = _SharedKernel.Patterns.ResultPattern.ValidationResult;

namespace API.Features.UserManagerFeature.DomainLayer.Services._Interfaces;

public interface IUserValidationService
{
    ValidationResult ValidateNewUser(string email, string password);
}