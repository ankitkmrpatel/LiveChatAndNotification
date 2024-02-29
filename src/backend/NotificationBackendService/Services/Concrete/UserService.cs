using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;
using NotificationBackendService.Models;
using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Services;

public class UserService : IUserService
{
    private readonly IRepo<User> userRepo;
    private readonly ITokenService tokenService;

    public UserService(IRepo<User> userRepo, ITokenService tokenService)
    {
        this.tokenService = tokenService;
        this.userRepo = userRepo;
    }

    public async Task<IReadOnlyCollection<User>> GetAllUsersAsync()
    {
        return await userRepo.GetAllAsync();
    }

    public async Task<User?> GetUserAsync(string username, string password)
    {
        username = username.ToUpper();
        var user = await userRepo.GetAsync(x => x.UserName.ToUpper().Equals(username)
            || x.Email.Equals(username));

        if(user?.Password == password)
            return user;

        return null;
    }

    public async Task<User?> GetUserAsync(string username)
    {
        var user = await userRepo.GetAsync(x => x.UserName.Equals(username)
            || x.Email.Equals(username));

        return user;
    }

    public async Task<ITokenResult> LoginAsync(User user)
    {
        var userTokenResult = await tokenService.GenerateTokensAsync(user);
        await tokenService.UserRefreshTokenAsync(user.Id, userTokenResult);
        
        return TokenResult.Create(userTokenResult);
    }

    public async Task<ValidationResultDto> LogoutAsync(Guid userId)
    {
        var isRefreshTokenRemoved = await tokenService.RemoveRefreshTokenAsync(userId);
        if (!isRefreshTokenRemoved)
        {
            return new ValidationResultDto() { IsSuccess = false, Message = "Unable to logout user!" };
        }

        return new ValidationResultDto() { IsSuccess = false, Message = "Logout Successfully!" };
    }
}


public interface ITokenResult
{
    public Guid AccessCode { get; init; }
    public string AccessToken { get; init; }
    public DateTime Expiry { get; init; }
    public string RefreshToken { get; }
}

public class TokenResult : ITokenResult
{
    private string? refreshToken;
    public TokenResult(string accessToken, DateTime expiry, Guid accessCode)
    {
        AccessToken = accessToken;
        Expiry = expiry;
        AccessCode = accessCode;
    }

    public string AccessToken { get; init; }
    public DateTime Expiry { get; init; }
    public Guid AccessCode { get; init; }
    public string RefreshToken { get => refreshToken; }

    public void SetRefreshToken(string token)
    {
        this.refreshToken = token;
    }

    public static ITokenResult Create(string accessToken, DateTime expiry, Guid accessCode)
    {
        return new TokenResult(accessToken, expiry, accessCode);
    }

    internal static ITokenResult Create(IUserTokenResultModel userTokenResult)
    {
        var tokenResult = new TokenResult(userTokenResult.AccessToken, userTokenResult.Expiry, userTokenResult.AccessCode);
        tokenResult.SetRefreshToken(userTokenResult.RefreshToken);

        return tokenResult;
    }
}
