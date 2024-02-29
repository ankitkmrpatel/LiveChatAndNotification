using NotificationBackendService.Hubs;

namespace NotificationBackendService.Extentions.ServiceCollection;

public static class SignalRExtentions
{
    public static void AddSignalRAndHubs(this IServiceCollection services)
    {
        services.AddScoped<EventHub>();
        services.AddScoped<NotificationHub>();
        services.AddScoped<INotificationHubService, NotificationHubService>();
        
        services.AddSignalR(hubOptions =>
        {
            hubOptions.EnableDetailedErrors = true;
            hubOptions.KeepAliveInterval = TimeSpan.FromMinutes(1);
        });
    }

    public static void AddSignalRHubs(this IEndpointRouteBuilder app)
    {
        app.MapHub<EventHub>("/eventHub");
        app.MapHub<NotificationHub>("/notificationHub");
    }
}
