using NotificationBackendService.Data.Entities;
using NotificationBackendService.Models;
using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Services;

public interface ITokenService
{
    Task<IUserTokenResultModel> GenerateTokensAsync(User user);
    Task UserRefreshTokenAsync(Guid userId, IUserTokenResultModel tokenResult);
    Task<ValidationResultDto> ValidateRefreshTokenAsync(RefreshTokenRequestDto refreshTokenRequest, Guid userId);
    Task<bool> RemoveRefreshTokenAsync(Guid userId);
}
