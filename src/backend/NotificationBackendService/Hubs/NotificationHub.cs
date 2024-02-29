using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using NotificationBackendService.Data.Entities;
using NotificationBackendService.Models.Dto;
using NotificationBackendService.Services;

namespace NotificationBackendService.Hubs;

[Authorize]
public class NotificationHub : Hub<INotificationHub>
{
    private readonly ICurrentUserIdentity currentUser;
    private readonly IUserHubConnectionService connectionService;

    public NotificationHub(ICurrentUserIdentity currentUser, IUserHubConnectionService connectionService)
    {
        this.currentUser = currentUser;
        this.connectionService = connectionService;
    }

    public override async Task OnConnectedAsync()
    {
        try
        {
            var userAgent = Context.Items["User-Agent"]?.ToString();
            await connectionService
                .AddUserConnection(currentUser, Context.ConnectionId, userAgent);
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine("Exception at Adding Connection From HUB", ex);
        }

        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception? exception)
    {
        try
        {
            await connectionService
                .RemoveUserConnection(currentUser, Context.ConnectionId);
        }
        catch (Exception e)
        {
            System.Diagnostics.Debug.WriteLine("Exception at Removing Connection From HUB", e);
        }

        await base.OnDisconnectedAsync(exception);
    }
}

public interface INotificationHub
{
    /// <summary>
    /// Send Notifioncation to Single User
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task SendNotification(EventItemDto message);

    /// <summary>
    /// Send Notification to All Users
    /// </summary>
    /// <param name="message"></param>
    /// <returns></returns>
    Task SendMessageToAllUser(EventItemDto message);
}

public interface INotificationHubService
{
    Task SendNotif(Guid userId, EventItemDto message);
    Task SendNotif(EventItemDto message);
}

public class NotificationHubService : INotificationHubService
{
    private readonly IHubContext<NotificationHub, INotificationHub> hubContext;
    private readonly IUserHubConnectionService userHubConnection;

    public NotificationHubService(IHubContext<NotificationHub, INotificationHub> hubContext,
        IUserHubConnectionService userHubConnection)
    {
        this.hubContext = hubContext;
        this.userHubConnection = userHubConnection;
    }

    public async Task SendNotif(Guid userId, EventItemDto message)
    {
        var connections = await userHubConnection.GetUserConnections(userId);

        foreach (var con in connections)
        {
            await hubContext.Clients.Client(con.ConnectionId)
                .SendNotification(message);
        }
    }

    public async Task SendNotif(EventItemDto message)
    {
        await hubContext.Clients.All
            .SendMessageToAllUser(message);
    }
}
