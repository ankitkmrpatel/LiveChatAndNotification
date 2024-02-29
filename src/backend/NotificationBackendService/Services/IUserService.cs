using NotificationBackendService.Data.Entities;
using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Services;

public interface IUserService
{
    Task<IReadOnlyCollection<User>> GetAllUsersAsync();
    Task<User?> GetUserAsync(string username);
    Task<User?> GetUserAsync(string username, string password);
    Task<ITokenResult> LoginAsync(User user); 
    Task<ValidationResultDto> LogoutAsync(Guid userId);
}