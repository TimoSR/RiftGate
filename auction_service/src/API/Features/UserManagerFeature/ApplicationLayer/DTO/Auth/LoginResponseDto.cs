namespace API.Features.UserManagerFeature.ApplicationLayer.DTO.Auth;

public class LoginResponseDto
{
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
}