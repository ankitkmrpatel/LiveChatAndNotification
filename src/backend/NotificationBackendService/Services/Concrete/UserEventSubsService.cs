using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Services;

public class UserEventSubsService : IUserEventSubsService
{
    private readonly IRepo<UserEventSubsSetting> eventUserRepo;

    public UserEventSubsService(IRepo<UserEventSubsSetting> eventUserRepo)
    {
        this.eventUserRepo = eventUserRepo;
    }

    public async Task SubsScribeShipment(UserEventSubsSetting eventUser)
    {
        await eventUserRepo.CreateAsync(eventUser);
        await eventUserRepo.SaveChangesAsync();
    }

    public async Task RemoveSubsScribeShipment(UserEventSubsSetting eventUser)
    {
        await eventUserRepo.DeleteAsync(eventUser.Id);
        await eventUserRepo.SaveChangesAsync();
    }

    public async Task<UserEventSubsSetting?> GetShipmentSubs(UserEventSubsSetting item)
    {
        var eventUser = await eventUserRepo.GetAsync(x => x.UserId.Equals(item.UserId)
            && x.EventModuleId.Equals(item.EventModuleId)
            && x.EventModuleType.Equals(item.EventModuleType));

        return eventUser;
    }
}
