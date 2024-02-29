using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Services;

public interface IEventService
{
    Task CreateAsync(Event @event);
    Task DeleteEvent(Guid id);
    Task<IReadOnlyCollection<Event>> GetAllEventsAsync();
    Task<Event?> GetEventyById(Guid id);
    Task UpdateEvent(Event @event);
    Task<IReadOnlyCollection<Event>> GetAllUndeliveredEvents();

    /// <summary>
    /// Get All User Using Event Module Id And Organization Mapping
    /// </summary>
    /// <param name="eventId"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<User>> GetApplicableUsers(Guid eventId);
}