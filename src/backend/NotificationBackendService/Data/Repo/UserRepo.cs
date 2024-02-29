using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Repo;

public class UserRepo : GenericRepository<User>
{
    public UserRepo(AppDataContext _context) : base(_context)
    {
    }
}
