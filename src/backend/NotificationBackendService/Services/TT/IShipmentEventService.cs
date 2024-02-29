using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Services
{
    public interface IShipmentEventService
    {
        Task<IReadOnlyCollection<Event>> GetShipmentEvents();
    }
}