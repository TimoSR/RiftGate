using System.ComponentModel.DataAnnotations;

namespace Application.DTO.UserManagement;

public class UserRegisterDto
{
    [Required]
    public string Email { get; set; }
    [Required]
    public string UserName { get; set; }
    [Required]
    public string Password { get; set; }
}