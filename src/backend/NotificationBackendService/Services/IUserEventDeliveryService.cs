using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Services;

public interface IUserEventDeliveryService
{
    Task<IReadOnlyCollection<UserEventReadStatus?>> GetAllUndeliveredEvents();
    Task UpdateUserEventDelivery(UserEventReadStatus userEventRead);
    Task UpdateUserEventDelivery(UserEventDeliveryStatus userEventDelivery);
}
