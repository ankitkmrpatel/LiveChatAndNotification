using NotificationBackendService.Hubs;
using NotificationBackendService.Services;

namespace NotificationBackendService.Extentions.ServiceCollection;

public static class AppServicesExtentions
{
    public static void AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IUserNotificationService, UserNotificationService>();  //Notificatoin Service And Login For User 
        services.AddScoped<INotificationSubsWorker, NotificationSubsWorker>();  //Event Subs Worker [User]
        services.AddScoped<IEventHubConcrete, EventHubConcrete>();  //Event Notificatioin Service
        
        services.AddScoped<IEventService, EventService>();  //Add Event to Db
        
        services.AddScoped<IShipmentService, ShipmentService>(); //Get User Shipments
        services.AddScoped<IShipmentEventService, ShipmentEventService>();  //Get Shipment Events
        services.AddScoped<IUserEventSubsService, UserEventSubsService>();  //Manage User Shipment Subs
        
        services.AddScoped<ITokenService, TokenService>();  //Token Service For Access Token and Refresh Token
        services.AddScoped<IUserService, UserService>();  //User Service for Login using Token Service
        
        services.AddScoped<IUserHubConnectionService, UserHubConnectionService>();  //User Notificaiton Service using Hub
        services.AddScoped<IUserEventService, UserEventService>();
        services.AddScoped<IUserEventDeliveryService, UserEventDeliveryService>();
    }
}
