using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;
using NotificationBackendService.Extentions;
using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Services;

public class UserHubConnectionService : IUserHubConnectionService
{
    private readonly IRepo<UserHubConnection> userHubConnectionRepo;

    public UserHubConnectionService(IRepo<UserHubConnection> userHubConnectionRepo)
    {
        this.userHubConnectionRepo = userHubConnectionRepo;
    }

    public async Task<IReadOnlyCollection<GetUserHubConnectionDto>> GetUserConnections(Guid userId)
    {
        var allConnection = await userHubConnectionRepo
            .GetAllAsync(x => x.UserId == userId);

        return allConnection.Select(x => x.AsDto())
            .ToList();
    }

    public async Task AddUserConnection(ICurrentUserIdentity currentUser, string connectionId, string? userAgent)
    {
        var conenction = new UserHubConnection()
        {
            UserId = currentUser.UserId,
            ConnectionId = connectionId,
            IsConnected = true,
            UserAgent = userAgent
        };

        await userHubConnectionRepo.CreateAsync(conenction);
        await userHubConnectionRepo.SaveChangesAsync();
    }

    public async Task RemoveUserConnection(ICurrentUserIdentity user, string connectionId)
    {
        var userConnection = await userHubConnectionRepo
            .GetAsync(x => x.UserId == user.UserId && x.ConnectionId == connectionId);

        if (userConnection == null) return;

        await userHubConnectionRepo.DeleteAsync(userConnection.Id);
        await userHubConnectionRepo.SaveChangesAsync();
    }
}
