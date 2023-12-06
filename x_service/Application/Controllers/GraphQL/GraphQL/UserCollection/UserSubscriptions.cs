using _SharedKernel.Patterns.ResultPattern;
using Application.Controllers.GraphQL.GraphQL._Interfaces;

namespace Application.Controllers.GraphQL.GraphQL.UserCollection;

public class UserSubscriptions : ISubscription
{
    [Subscribe]
    [Topic]
    public ServiceResult OnUserRegistered([EventMessage] ServiceResult registeredUser) => registeredUser;
}
