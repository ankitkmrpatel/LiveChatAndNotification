using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Services;

public class EventService : IEventService
{
    private readonly IRepo repo;
    private readonly IRepo<Event> eventRepo;

    public EventService(IRepo repo, IRepo<Event> eventRepo)
    {
        this.repo = repo;
        this.eventRepo = eventRepo;
    }

    public async Task CreateAsync(Event @event)
    {
        await eventRepo.CreateAsync(@event);
        await eventRepo.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<Event>> GetAllEventsAsync()
    {
        return await eventRepo.GetAllAsync();
    }

    public async Task<Event?> GetEventyById(Guid id)
    {
        return await eventRepo.GetAsync(id);
    }

    public async Task UpdateEvent(Event @event)
    {
        await eventRepo.UpdateAsync(@event);
        await eventRepo.SaveChangesAsync();
    }

    public async Task DeleteEvent(Guid id)
    {
        await eventRepo.DeleteAsync(id);
        await eventRepo.SaveChangesAsync();
    }

    public async Task<IReadOnlyCollection<Event>> GetAllUndeliveredEvents()
    {
        var allUndeliveredEvents = await eventRepo
            .GetAllAsync(x => !x.IsDevliverd);

        return allUndeliveredEvents;
    }

    public async Task<IReadOnlyCollection<User>> GetApplicableUsers(Guid eventId)
    {
        var eventUsersQuery = $@"SELECT u.Id,
                       u.Name,
                       u.UserName,
                       u.Email,
                       u.Organizations
                  FROM Users u
                  Left Join Shipments s
                    on u.Organizations in (s.CneeCode, s.CnorCode, s.NotifCode)
                  Left Join Events e
                    on e.EventModuleId = s.ShipmentId
                  Where e.Id = '{eventId.ToString().ToUpper()}'";  //Combine Other Module - Order/Transaction/RMS etc

        var result = await repo.ExecuteQueryAsync(eventUsersQuery,
            x => new User()
            {
                Id = Guid.Parse(x[0].ToString() ?? throw new Exception()),
                Name = (string)x[1],
                UserName = (string)x[2],
                Email = (string)x[3],
                Organizations = (string)x[4],
            });

        return result;
    }

}
