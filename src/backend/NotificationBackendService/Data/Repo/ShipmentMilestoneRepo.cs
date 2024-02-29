using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Repo;

public class ShipmentMilestoneRepo : GenericRepository<ShipmentMilestone>
{
    public ShipmentMilestoneRepo(AppDataContext _context) : base(_context)
    {
    }
}
