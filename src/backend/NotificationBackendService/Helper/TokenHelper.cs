using Microsoft.IdentityModel.Tokens;
using static NotificationBackendService.Constants;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Security.Cryptography;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Helper;

public class TokenHelper
{
    public static async Task<string> GenerateUserToken(User user, Guid accessCode, DateTime expiry, string issuer, string securityKey, string audience)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name.ToString()),
            new Claim(ClaimTypes.PrimarySid, user.UserName.ToString()),
            new Claim(ClaimTypes.Email, user.Email.ToString()),
            new Claim(ClaimTypes.Sid, accessCode.ToString()),
            new Claim(JwtClaims.IsBlock, user.IsBlocked.ToString(), ClaimValueTypes.Boolean)
        };


        var key = Encoding.UTF8.GetBytes(securityKey);
        var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

        // Create and sign the token
        var jwtSecurityToken = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiry,
            signingCredentials: signingCredentials
        );

        jwtSecurityToken.Header.Add(Jwt.TokenHeaderKIDName, audience);

        var token = await Task.Run(() => new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken));
        return token;
    }

    public static async Task<string> GenerateRefreshToken()
    {
        var secureRandomBytes = new byte[32];
        using var randomNumberGenerator = RandomNumberGenerator.Create();
        await Task.Run(() => randomNumberGenerator.GetBytes(secureRandomBytes));
        var refreshToken = Convert.ToBase64String(secureRandomBytes);
        return refreshToken;
    }
}
