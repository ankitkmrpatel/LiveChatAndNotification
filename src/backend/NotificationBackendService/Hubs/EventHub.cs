using Microsoft.AspNetCore.SignalR;
using NotificationBackendService.Models.Dto;
using NotificationBackendService.Services;

namespace NotificationBackendService.Hubs;

public class EventHub : Hub
{
    private readonly ICurrentUserIdentity currentUser;
    private readonly IUserHubConnectionService connectionService;
    public EventHub(ICurrentUserIdentity currentUser, IUserHubConnectionService connectionService) : base()
    {
        this.currentUser = currentUser;
        this.connectionService = connectionService;
    }

    public async Task SendNotif(string user, EventItemDto message)
    {
        await Clients.All.SendAsync("SendNotification", message);
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

public interface IEventHubConcrete
{
    Task SendNotif(string username, EventItemDto message);
}

public class EventHubConcrete : IEventHubConcrete
{
    private readonly IHubContext<EventHub> eventHub;

    public EventHubConcrete(IHubContext<EventHub> eventHub)
    {
        this.eventHub = eventHub;
    }

    public async Task SendNotif(string username, EventItemDto message)
    {
        //Get All User Connection and Send to User
        await eventHub.Clients.All.SendAsync("SendNotification", message);
    }
}
