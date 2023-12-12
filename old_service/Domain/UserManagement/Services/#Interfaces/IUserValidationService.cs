using _SharedKernel.Patterns.ResultPattern;

namespace Domain.UserManagement.Services._Interfaces;

public interface IUserValidationService
{
    ValidationResult ValidateNewUser(string email, string password);
}