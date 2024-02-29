using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Services
{
    public interface IUserEventSubsService
    {
        Task<UserEventSubsSetting?> GetShipmentSubs(UserEventSubsSetting item);
        Task RemoveSubsScribeShipment(UserEventSubsSetting eventUser);
        Task SubsScribeShipment(UserEventSubsSetting eventUser);
    }
}