using NotificationBackendService.Services;

namespace NotificationBackendService.Models;

public class UserTokenResultModel : IUserTokenResultModel
{
    public Guid AccessCode { get; init; }
    
    public string AccessToken { get; init; }
    public DateTime Expiry { get; init; }
    
    public string RefreshSecret { get; init; }
    public string RefreshToken { get; init; }
    public string RefreshTokenHash { get; init; }
}

public interface IUserTokenResultModel : ITokenResult
{
    public string RefreshSecret { get; init; }
    public string RefreshTokenHash { get; init; }
}
