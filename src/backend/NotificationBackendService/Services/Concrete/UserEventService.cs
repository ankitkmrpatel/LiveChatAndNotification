using Microsoft.Extensions.Logging;
using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;
using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Services;

public class UserEventService : IUserEventService
{
    private readonly IRepo repo;
    private readonly IRepo<UserEvent> userEventRepo;
    private readonly IRepo<UserEventReadStatus> eventReadRepo;
    private readonly IRepo<UserEventDeliveryStatus> eventDeliveryRepo;

    public UserEventService(IRepo repo, 
        IRepo<UserEvent> userEventRepo, 
        IRepo<UserEventReadStatus> eventReadRepo, 
        IRepo<UserEventDeliveryStatus> eventDeliveryRepo)
    {
        this.repo = repo;
        this.userEventRepo = userEventRepo;
        this.eventReadRepo = eventReadRepo;
        this.eventDeliveryRepo = eventDeliveryRepo;
    }

    public async Task SaveNewUserEvent(UserEvent userEvent)
    {
        var readEvent = await eventReadRepo.CreateAsync(new UserEventReadStatus()
        {
            EventId = userEvent.EventId,
            UserId = userEvent.UserId,
            NotificationRead = false,
            NotificationSent = false,
            Emailed = false,
            SMSSent = false,
        });
        await eventReadRepo.SaveChangesAsync();

        userEvent.UserEventReadId = readEvent.Id;

        await userEventRepo.CreateAsync(userEvent);
        await userEventRepo.SaveChangesAsync();
    }

    public async Task MarkDeliveryForMode(UserEventDeliveryStatus deliveryStatus)
    {
        await eventDeliveryRepo.CreateAsync(deliveryStatus);
        await eventDeliveryRepo.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<Event?>> GetUserEventsData(Guid userId)
    {
        var userEventsQuery = $@"SELECT e.Id,
                       e.EventType,
                       e.EventModuleId,
                       e.EventModuleType,
                       e.EventDesctipion,
                       e.EventData
                  FROM Events e
                  Inner Join UserEvents ue
                    on  ue.EventId = e.Id
                  Inner Join Users u
                    on u.Id = ue.UserId
                  Where u.Id = '{userId}'";

        var result = await repo.ExecuteQueryAsync(userEventsQuery,
            x => new Event()
            {
                Id = Guid.Parse(x[0].ToString() ?? throw new Exception()),
                EventType = (string)x[1],
                EventModuleId = (string)x[2],
                EventModuleType = (string)x[3],
                EventDesctipion = (string)x[4],
                EventData = Convert.ToDateTime(x[5])
            });

        return result;
    }

    public Task ValidateUserEventSubscription(User user, EventItemDto eventItem)
    {
        throw new NotImplementedException();
    }

    public async Task<IReadOnlyCollection<UserEvent?>> GetUserEvents(Guid userId)
    {
        return await userEventRepo
            .GetAllAsync(x => x.UserId == userId, "Event");
    }

    public async Task<UserEvent?> GetUserEventById(Guid userId, Guid eventId)
    {
        return await userEventRepo
           .GetAsync(x => x.UserId == userId && x.EventId == eventId);
    }
}
