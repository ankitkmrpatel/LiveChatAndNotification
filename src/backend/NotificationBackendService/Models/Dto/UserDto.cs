using NotificationBackendService.Services;
using System.Text.Json.Serialization;

namespace NotificationBackendService.Models.Dto;

public class LoginRequestDto
{
    public string Username { get; set; }
    public string Password { get; set; }
}

public class RefreshTokenRequestDto
{
    public string Username { get; set; }
    public string RefreshToken { get; set; }
}

public class GetUserTokenDto
{
    public string Name { get; set; }
    public string Username { get; set; }
    public ITokenResult Token { get; set; }
    public string Organizations { get; set; }
    public string Email { get; set; }
}

public class ValidateRefreshTokenResponse
{
    [JsonIgnore()]
    public bool Success { get; set; }
    public string Message { get; set; }
}
