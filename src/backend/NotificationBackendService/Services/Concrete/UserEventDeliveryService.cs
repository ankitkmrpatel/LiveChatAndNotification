using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Services;

public class UserEventDeliveryService : IUserEventDeliveryService
{
    private readonly IRepo<UserEventReadStatus> userEvtReadRepo;
    private readonly IRepo<UserEventDeliveryStatus> userEvtDeliveryRepo;

    public UserEventDeliveryService(IRepo<UserEventReadStatus> userEvtReadRepo,
        IRepo<UserEventDeliveryStatus> userEvtDeliveryRepo)
    {
        this.userEvtReadRepo = userEvtReadRepo;
        this.userEvtDeliveryRepo = userEvtDeliveryRepo;
    }

    public async Task<IReadOnlyCollection<UserEventReadStatus?>> GetAllUndeliveredEvents()
    {
        return await userEvtReadRepo
            .GetAllAsync(x => !x.Emailed || !x.SMSSent);
    }

    public async Task<IReadOnlyCollection<UserEventReadStatus?>> GetAllNOTSentEvents()
    {
        return await userEvtReadRepo
            .GetAllAsync(x => !x.NotificationSent);
    }

    public async Task UpdateUserEventDelivery(UserEventReadStatus userEventRead)
    {
        await userEvtReadRepo.UpdateAsync(userEventRead);
        await userEvtReadRepo.SaveChangesAsync();
    }

    public async Task UpdateUserEventDelivery(UserEventDeliveryStatus userEventDelivery)
    {
        await userEvtDeliveryRepo.UpdateAsync(userEventDelivery);
        await userEvtDeliveryRepo.SaveChangesAsync();
    }
}
