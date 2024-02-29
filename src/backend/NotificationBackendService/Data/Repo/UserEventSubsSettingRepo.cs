using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Repo;

public class UserEventSubsSettingRepo : GenericRepository<UserEventSubsSetting>
{
    public UserEventSubsSettingRepo(AppDataContext _context) : base(_context)
    {
    }
}
