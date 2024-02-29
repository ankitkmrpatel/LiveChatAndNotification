using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Services;

public interface IUserHubConnectionService
{
    Task<IReadOnlyCollection<GetUserHubConnectionDto>> GetUserConnections(Guid userId);
    Task AddUserConnection(ICurrentUserIdentity currentUser, string connectionId, string? userAgent);
    Task RemoveUserConnection(ICurrentUserIdentity user, string connectionId);
}
