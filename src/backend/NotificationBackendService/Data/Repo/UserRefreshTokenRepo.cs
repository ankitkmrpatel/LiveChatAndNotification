using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Repo
{
    public class UserRefreshTokenRepo : GenericRepository<UserRefreshToken>
    {
        public UserRefreshTokenRepo(AppDataContext _context) : base(_context)
        {
        }
    }
}
