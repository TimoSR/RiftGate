using _SharedKernel.Patterns.DataTransferObjects;

namespace Application.AppServices.xFeature.V1.AppServices.CommandHandlers.Register;

public class RegisterRequest : IRequest
{
    public Guid Id { get; }
}