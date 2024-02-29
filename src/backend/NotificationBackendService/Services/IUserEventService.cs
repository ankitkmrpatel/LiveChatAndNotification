using NotificationBackendService.Data.Entities;
using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Services;

public interface IUserEventService
{
    Task SaveNewUserEvent(UserEvent userEvent);

    /// <summary>
    /// Get All User's Events
    /// </summary>
    /// <param name="userId"></param>
    /// <returns></returns>
    Task<IReadOnlyCollection<UserEvent?>> GetUserEvents(Guid userId);
    Task<UserEvent?> GetUserEventById(Guid userId, Guid eventId);
    Task<IReadOnlyCollection<Event?>> GetUserEventsData(Guid userId);
}
