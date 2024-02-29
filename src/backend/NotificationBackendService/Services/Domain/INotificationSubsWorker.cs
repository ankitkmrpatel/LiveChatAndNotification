using NotificationBackendService.Data.Entities;
using NotificationBackendService.Hubs;
using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Services;

/// <summary>
/// Event To User Mapping Worker
/// </summary>
public interface INotificationSubsWorker
{
    /// <summary>
    /// Get Event And Map to User 
    /// </summary>
    /// <param name="eventItem"></param>
    /// <returns></returns>
    public Task Process(EventItemDto eventItem);
    public Task Process(Guid userId, EventItemDto eventItem);
    
    /// <summary>
    /// Read User Event And Send to User Via Delivery Mode Subs
    /// </summary>
    /// <param name="eventItem"></param>
    /// <returns></returns>
    public Task DeliverEventToUser(EventItemDto eventItem);
}

public class NotificationSubsWorker : INotificationSubsWorker
{
    private readonly IUserNotificationService notificationService;
//#warning Change Here to Send to User Using Connection Id
    private readonly INotificationHubService eventHub;

    public NotificationSubsWorker(IUserNotificationService notificationService, INotificationHubService eventHub)
    {
        this.notificationService = notificationService;
        this.eventHub = eventHub;
    }

    public async Task Process(EventItemDto eventItem)
    {
        //Get All User Using Module Id
        var users = await notificationService.GetApplicableUsers(eventItem.EventId);

        //Save The Event to UserEventTable
        foreach (var user in users)
        {
            await notificationService.CreateNewEventForUser(user, eventItem);
            await Process(user.Id, eventItem);
        }
    }


    public async Task Process(Guid userId, EventItemDto eventItem)
    {
        await eventHub.SendNotif(userId, eventItem);
    }

    public Task DeliverEventToUser(EventItemDto eventItem)
    {
        //Read Event From UserEvent Table
        //Read All Delivery Mode Using SHipment Subs
        //Process All User Delivery Mode
        //Update the EventDeliveryStatus Table
        throw new NotImplementedException();
    }
}
