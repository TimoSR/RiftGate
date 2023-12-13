using System.ComponentModel.DataAnnotations;
using ValidationResult = _SharedKernel.Patterns.ResultPattern.ValidationResult;

namespace x_serviceAPI.Features.UserManagerFeature.DomainLayer.Services._Interfaces;

public interface IUserValidationService
{
    ValidationResult ValidateNewUser(string email, string password);
}