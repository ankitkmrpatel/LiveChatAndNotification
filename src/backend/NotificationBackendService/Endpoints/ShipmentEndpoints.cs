using Microsoft.AspNetCore.Authorization;
using NotificationBackendService.Data.Entities;
using NotificationBackendService.Extentions;
using NotificationBackendService.Services;

namespace NotificationBackendService.Endpoints;

public static class ShipmentEndpoints
{
    public static void MapShipmentEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("api/shipments", GetAllShipments)
               .WithName("GetAllShipments").WithDisplayName("Get All User's Shipment")
               .Produces<IReadOnlyCollection<Shipment>>(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status400BadRequest)
               .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("api/shipments/{shipmentNumber}", GetShipmentDetails)
               .WithName("GetShipmentDetails").WithDisplayName("Get User Shipment Details")
               .Produces<IReadOnlyCollection<Shipment>>(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status400BadRequest)
               .Produces(StatusCodes.Status404NotFound)
               .Produces(StatusCodes.Status500InternalServerError);
    }

    [Authorize]
    private static async Task<IResult> GetAllShipments(IShipmentService shipmentService)
    {
        var entities = await shipmentService.GetUserShipment();
        var items = entities.Select(x => x.AsDto());

        return Results.Ok(items);
    }

    [Authorize]
    private static async Task<IResult> GetShipmentDetails(IShipmentService shipmentService, string shipmentNumber)
    {
        if (string.IsNullOrEmpty(shipmentNumber))
            return Results.BadRequest();

        var entity = await shipmentService.GetUserShipment(shipmentNumber);

        if (entity == null)
            return Results.NotFound();

        var shipmentMilestone = await shipmentService.GetShipmentMilestone(shipmentNumber);
        var item = entity.AsDto(shipmentMilestone);

        return Results.Ok(item);
    }
}
