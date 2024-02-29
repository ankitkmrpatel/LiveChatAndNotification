using Microsoft.IdentityModel.Tokens;
using static NotificationBackendService.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using NotificationBackendService.Models;
using NotificationBackendService.Data;
using NotificationBackendService.Helper;
using NotificationBackendService.Data.Entities;
using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IRepo<UserRefreshToken> userTokenRepo;

    public TokenService(IConfiguration configuration, IRepo<UserRefreshToken> userTokenRepo)
    {
        this._configuration = configuration;
        this.userTokenRepo = userTokenRepo;
    }

    public async Task<IUserTokenResultModel> GenerateTokensAsync(User user)
    {
        string issuer = _configuration[Jwt.IssuerName] ?? throw new ArgumentException("Jwt Token Issuer NOT Found.");
        string securityKey = _configuration[Jwt.SecretKey] ?? throw new ArgumentException("Jwt Token Security Key NOT Found.");
        string audience = _configuration[Jwt.AudienceName] ?? throw new ArgumentException("Jwt Token Audiences NOT Found.");

        // Get the token config from appsettings.json
        if (!int.TryParse(_configuration[Jwt.ExpiryDurationMinsName], out int expiryDuration))
        {
            expiryDuration = 10;
        }

        var accessCode = Guid.NewGuid();
        DateTime expiry = DateTime.Now.Add(TimeSpan.FromMinutes(expiryDuration));
        expiry = DateTime.Now.AddDays(expiryDuration);

        string token = await TokenHelper.GenerateUserToken(user, accessCode, expiry, issuer, securityKey, audience);

        var refreshToken = await TokenHelper.GenerateRefreshToken();
        var salt = PasswordHelper.GetSecureSalt();
        var refreshTokenHashed = PasswordHelper.HashUsingPbkdf2(refreshToken, salt);

        // return the token and when it expires
        var tokenResult = new UserTokenResultModel()
        {
            AccessCode = accessCode,
            AccessToken = token,
            Expiry = expiry,
            RefreshToken = refreshToken,
            RefreshTokenHash = refreshTokenHashed,
            RefreshSecret = Convert.ToBase64String(salt)
        };

        return tokenResult;
    }

    //Remove Refresh Token From Db
    public async Task<bool> RemoveRefreshTokenAsync(Guid userId)
    {
        var userRefreshTokens = await userTokenRepo.GetAllAsync(x => x.UserId == userId);

        if (userRefreshTokens != null && userRefreshTokens.Count > 0)
        {
            var removeTasks = new List<Task>();
            foreach (var refreshToken in userRefreshTokens)
            {
                removeTasks.Add(userTokenRepo.DeleteAsync(refreshToken.Id));
            }

            await Task.WhenAll(removeTasks);
            return await userTokenRepo.SaveChangesAsync();
        }

        return true;
    }

    //Add Refresh Token Into Db
    public async Task UserRefreshTokenAsync(Guid userId, IUserTokenResultModel tokenResult)
    {
        await RemoveRefreshTokenAsync(userId);
        await userTokenRepo.CreateAsync(new UserRefreshToken()
        {
            UserId = userId,

            ExpiryDate = DateTime.Now.AddDays(30),
            TokenHash = tokenResult.RefreshTokenHash,
            TokenSalt = tokenResult.RefreshSecret,

            TS = DateTime.Now,
        });

        await userTokenRepo.SaveChangesAsync();
    }

    //Validate Refresh Token
    public async Task<ValidationResultDto> ValidateRefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequest, Guid userId)
    {
        var refreshToken = await userTokenRepo.GetAsync(x => x.UserId == userId);

        var response = new ValidationResultDto();
        if (refreshToken == null)
        {
            response.IsSuccess = true;
            response.Message = "Invalid session or user is already logged out";
            return response;
        }

        var refreshTokenToValidateHash = PasswordHelper.HashUsingPbkdf2(refreshTokenRequest.RefreshToken, Convert.FromBase64String(refreshToken.TokenSalt));
        if (refreshToken.TokenHash != refreshTokenToValidateHash)
        {
            response.IsSuccess = false;
            response.Message = "Invalid refresh token";
            return response;
        }

        if (refreshToken.ExpiryDate < DateTime.Now)
        {
            response.IsSuccess = false;
            response.Message = "Refresh token has expired";
            return response;
        }

        response.IsSuccess = true;
        return response;
    }

}
