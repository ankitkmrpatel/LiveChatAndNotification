using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Repo;

public class ShipmentRepo : GenericRepository<Shipment>
{
    public ShipmentRepo(AppDataContext _context) : base(_context)
    {
    }
}
