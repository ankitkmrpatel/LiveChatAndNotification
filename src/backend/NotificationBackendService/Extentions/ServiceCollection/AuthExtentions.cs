using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace NotificationBackendService.Extentions.ServiceCollection;

public static class AuthExtentions
{
    public static void AddAuthenticationWithJwt(this IServiceCollection services, IConfiguration config)
    {
        services.AddAuthentication(o =>
        {
            o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(o =>
        {
            o.Events = new JwtBearerEvents()
            {
                OnMessageReceived = (context) =>
                {
                    var accessToken = context.Request.Query["access_token"];
                    if (!string.IsNullOrEmpty(accessToken))
                    {
                        context.Token = accessToken;
                    }

                    return Task.CompletedTask;
                }
            };
            o.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = config[Constants.Jwt.IssuerName],
                ValidAudience = config[Constants.Jwt.AudienceName],
                IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(config[Constants.Jwt.SecretKey])),
                ClockSkew = TimeSpan.FromSeconds(5),
            };
        });

        services.AddAuthorization();
    }

    public static void AddApplicationAuth(this IApplicationBuilder app)
    {
        app.UseAuthentication();
        app.UseAuthorization();
    }
}
