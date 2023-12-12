using Domain.DomainEvents;
using MediatR;

namespace Application.AppServices.V1.xService.Commands.Register;

public class RegisterEventHandler : INotificationHandler<UserRegisteredEvent>
{
    // Dependencies necessary to handle the event (e.g., an email service)
    private readonly IEmailService _emailService;

    public RegisterEventHandler(IEmailService emailService)
    {
        _emailService = emailService;
    }
    
    public async Task Handle(UserRegisteredEvent notification, CancellationToken cancellationToken)
    {
        // Example action: Send a confirmation email
        await _emailService.SendConfirmationEmail(notification.Email);
    }
}
