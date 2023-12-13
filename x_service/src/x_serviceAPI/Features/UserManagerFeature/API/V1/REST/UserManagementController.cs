using Application.DTO.UserManagement;
using CodingPatterns.ApplicationLayer.ServiceResultPattern._Enums;
using Infrastructure.Swagger;
using Infrastructure.Swagger.Attributes;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using x_serviceAPI.Features.UserManagerFeature.ApplicationLayer.AppServices._Interfaces;

namespace x_serviceAPI.Features.UserManagerFeature.API.V1.REST;

[ApiController]
[Route("api/v1/[controller]")]
[SwaggerDoc("UserManagement")]
[ApiVersion("1.0")]
[Authorize]
public class UserManagementController : ControllerBase
{
    private readonly IUserService _userService;

    public UserManagementController(IUserService userService)
    {
        _userService = userService;
    }

    /// <summary>
    /// Register a new user
    /// </summary>
    [AllowAnonymous]
    [HttpPost("RegisterUser")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> RegisterUser(UserRegisterDto newUserDto)
    {
        var result = await _userService.RegisterAsync(newUserDto);

        if (result.IsSuccess)
        {
            return Ok(new { result.Messages });
        }

        return result.ErrorType switch
        {
            ServiceErrorType.BadRequest => BadRequest(new { Message = result.Messages })
        };
    }
    
    /// <summary>
    /// Register a new user
    /// </summary>
    [HttpGet("GetUserByEmail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserByEmail(string email)
    {
        var result = await _userService.GetUserByEmailAsync(email);

        if (result.IsSuccess)
        {
            return Ok(new { result.Data });
        }   
        
        return BadRequest(new { Message = result.Messages});
    }

    /// <summary>
    /// Delete a user
    /// </summary>
    [HttpDelete("DeleteUserByEmail")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteUserByEmail(string email)
    {
        var result = await _userService.DeleteUserByEmailAsync(email);

        if (result.IsSuccess)
        {
            return Ok(new { Message = "User deleted successfully" });
        }

        return BadRequest(new { Message = "Failed to delete user" });
    }
}