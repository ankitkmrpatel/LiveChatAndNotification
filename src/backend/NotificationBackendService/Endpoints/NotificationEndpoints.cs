using Microsoft.AspNetCore.Authorization;
using NotificationBackendService.Data.Entities;
using NotificationBackendService.Extentions;
using NotificationBackendService.Services;

namespace NotificationBackendService.Endpoints
{
    public static class NotificationEndpoints
    {
        public static void MapNotificationEndpoints(this IEndpointRouteBuilder app)
        {
            app.MapGet("api/notifications", GetAllUserEvents)
                   .WithName("GetAllUserEvents").WithDisplayName("Get All User's Event")
                   .Produces<IReadOnlyCollection<Event>>(StatusCodes.Status200OK)
                   .Produces(StatusCodes.Status400BadRequest)
                   .Produces(StatusCodes.Status500InternalServerError);
        }

        [Authorize]
        private static async Task<IResult> GetAllUserEvents(IShipmentEventService shipmentEvtService)
        {
            var entities = await shipmentEvtService.GetShipmentEvents();
            var items = entities.Select(x => x.AsDto());

            return Results.Ok(items);
        }
    }
}
