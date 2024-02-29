using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Services;

public class ShipmentService : IShipmentService
{
    private readonly IRepo<Shipment> repo;
    private readonly IRepo<ShipmentMilestone> milestoneRepo;
    private readonly IUserService userService;
    private readonly ICurrentUserIdentity currentUser;

    public ShipmentService(IRepo<Shipment> repo, IRepo<ShipmentMilestone> milestoneRepo, IUserService userService, ICurrentUserIdentity currentUser)
    {
        this.repo = repo;
        this.milestoneRepo = milestoneRepo;
        this.userService = userService;
        this.currentUser = currentUser;
    }

    public async Task<IReadOnlyCollection<Shipment>> GetUserShipment()
    {
        var user = await GetUserOrgCodes();
        _ = user ?? throw new ArgumentException(nameof(user));

        var userOrg = user.Organizations;

        var shipments = await repo.GetAllAsync(x => (!string.IsNullOrEmpty(x.CnorCode) &&
                userOrg.Contains(x.CnorCode))
                || (!string.IsNullOrEmpty(x.CneeCode) &&
                userOrg.Contains(x.CneeCode))
                || (!string.IsNullOrEmpty(x.NotifCode) &&
                userOrg.Contains(x.NotifCode)));

        return shipments;
    }

    public async Task<Shipment?> GetUserShipment(string id)
    {
        var user = await GetUserOrgCodes();
        _ = user ?? throw new ArgumentException(nameof(user));

        var userOrg = user.Organizations;

        var shipment = await repo.GetAsync(x => x.ShipmentId == id &&
                (
                !string.IsNullOrEmpty(x.CnorCode) && userOrg.Contains(x.CnorCode)
                || !string.IsNullOrEmpty(x.CneeCode) && userOrg.Contains(x.CneeCode)
                || !string.IsNullOrEmpty(x.NotifCode) && userOrg.Contains(x.NotifCode)
                )
                );

        return shipment;
    }

    public async Task<IReadOnlyCollection<ShipmentMilestone>?> GetShipmentMilestone(string shipmentId)
    {
        return await milestoneRepo
            .GetAllAsync(x => x.ShipmentId == shipmentId);
    }

    private async Task<User?> GetUserOrgCodes()
    {
        return await userService.GetUserAsync(currentUser.UserName);
    }

}
