using NotificationBackendService.Data.Entities;
using NotificationBackendService.Data;
using NotificationBackendService.Repo;

namespace NotificationBackendService.Extentions.ServiceCollection;

public static class RepositoryExtentions
{
    public static void AddRepository(this IServiceCollection services)
    {
        services.AddScoped<IRepo, GenericRepository>();
        services.AddScoped<IRepo<Event>, EventRepo>();

        services.AddScoped<IRepo<Shipment>, ShipmentRepo>();
        services.AddScoped<IRepo<ShipmentMilestone>, ShipmentMilestoneRepo>();

        services.AddScoped<IRepo<User>, UserRepo>();
        services.AddScoped<IRepo<UserRefreshToken>, UserRefreshTokenRepo>();
        services.AddScoped<IRepo<UserHubConnection>, UserHubConnectionRepo>();

        services.AddScoped<IRepo<UserEvent>, UserEventRepo>();
        services.AddScoped<IRepo<UserEventReadStatus>, UserEventReadStatusRepo>();
        services.AddScoped<IRepo<UserEventDeliveryStatus>, UserEventDeliveryStatusRepo>();
        
        services.AddScoped<IRepo<UserEventSubsSetting>, UserEventSubsSettingRepo>();
    }
}
