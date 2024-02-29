using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Repo
{
    public class UserHubConnectionRepo : GenericRepository<UserHubConnection>
    {
        public UserHubConnectionRepo(AppDataContext _context) : base(_context)
        {
        }
    }
}
