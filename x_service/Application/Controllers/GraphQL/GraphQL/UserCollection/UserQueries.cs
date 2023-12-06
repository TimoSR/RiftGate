using _SharedKernel.Patterns.ResultPattern;
using Application.AppServices._Interfaces;
using Application.Controllers.GraphQL.GraphQL._Interfaces;
using Application.DTO.UserManagement;

namespace Application.Controllers.GraphQL.GraphQL.UserCollection;

public class UserQueries : IQuery
{
    private readonly IUserService _userService;
    
    public UserQueries(IUserService userService)
    {
        _userService = userService;
    }
    
    public async Task<ServiceResult<UserDto>> GetUserByEmail(string email)
    {
        var result = await _userService.GetUserByEmailAsync(email);
        return result;
    }
}