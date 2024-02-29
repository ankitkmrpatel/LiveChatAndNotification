using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Services
{
    public interface IShipmentService
    {
        Task<IReadOnlyCollection<Shipment>> GetUserShipment();
        Task<Shipment?> GetUserShipment(string id);
        Task<IReadOnlyCollection<ShipmentMilestone>?> GetShipmentMilestone(string shipmentId);
    }
}