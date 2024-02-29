using NotificationBackendService.Endpoints;

namespace NotificationBackendService.Extentions.ServiceCollection;

public static class EndpointExtentions
{
    public static void AddEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapEventEndpoints();
        app.MapShipmentEndpoints();
        app.MapShipmentSubsEndpoints();
        app.MapUserEndpoints();
        app.MapUiHelperEndpoints();
    }
}
