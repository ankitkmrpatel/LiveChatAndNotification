using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Repo;

public class UserEventRepo : GenericRepository<UserEvent>
{
    public UserEventRepo(AppDataContext _context) : base(_context)
    {
    }
}

public class UserEventReadStatusRepo : GenericRepository<UserEventReadStatus>
{
    public UserEventReadStatusRepo(AppDataContext _context) : base(_context)
    {
    }
}

public class UserEventDeliveryStatusRepo : GenericRepository<UserEventDeliveryStatus>
{
    public UserEventDeliveryStatusRepo(AppDataContext _context) : base(_context)
    {
    }
}
