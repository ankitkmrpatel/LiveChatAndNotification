using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Repo
{
    public class EventRepo : GenericRepository<Event>
    {
        public EventRepo(AppDataContext _context) : base(_context)
        {
        }
    }
}
