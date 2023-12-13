using CodingPatterns.ApplicationLayer.DataTransferObjects;

namespace API.Features.AFeature.ApplicationLayer.AppServices.CommandHandlers.Register;

public class RegisterRequest : IRequest
{
    public Guid Id { get; }
}