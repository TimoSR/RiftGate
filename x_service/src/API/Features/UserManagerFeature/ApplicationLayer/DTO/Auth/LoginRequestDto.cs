namespace API.Features.UserManagerFeature.ApplicationLayer.DTO.Auth;

public class LoginRequestDto
{
    private string _email;
    public string Email 
    { 
        get => _email; 
        set => _email = value?.ToLowerInvariant(); 
    }
    public string Password { get; set; }
}