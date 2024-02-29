using NotificationBackendService.Data.Entities;
using NotificationBackendService.Extentions;
using NotificationBackendService.Models.Dto;
using NotificationBackendService.Services;

namespace NotificationBackendService.Endpoints;

public static class ShipmentSubscriptionEndpoints
{
    public static void MapShipmentSubsEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/shipmentsubs", CreateNewShipmentSubs)
            .WithName("CreateNewShipmentSubs").WithDisplayName("Create New Shipment Subs")
            .Produces<GetShipmentSubsDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapPut("api/shipmentsubs", DeleteShipmentSubs)
            .WithName("DeleteShipmentSubs").WithDisplayName("Delete Shipment Subs")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    /// <summary>
    /// Create New Catalog Item
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="newItem"></param>
    /// <returns></returns>
    private static async Task<IResult> CreateNewShipmentSubs(IUserEventSubsService shipmentSubsService, ICurrentUserIdentity currentUser, CreateNewShipmentSubsDto newItem)
    {
        if (string.IsNullOrEmpty(newItem.ShipmentId))
            return Results.BadRequest();

        var entity = new UserEventSubsSetting()
        {
            EventModuleId = "SHP",
            EventModuleType = newItem.ShipmentId,
            UserId = currentUser.UserId
        };

        await shipmentSubsService.SubsScribeShipment(entity);
        var item = entity.AsDto();

        return Results.Created($"api/shipmentsubs/{entity.Id}", item);
    }

    /// <summary>
    /// Remove Shiment Item
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="newItem"></param>
    /// <returns></returns>
    private static async Task<IResult> DeleteShipmentSubs(IUserEventSubsService shipmentSubsService, ICurrentUserIdentity currentUser, CreateNewShipmentSubsDto newItem)
    {
        if (string.IsNullOrEmpty(newItem.ShipmentId))
            return Results.BadRequest();

        var entity = new UserEventSubsSetting()
        {
            EventModuleId = "SHP",
            EventModuleType = newItem.ShipmentId,
            UserId = currentUser.UserId,
        };

        var eventUser = await shipmentSubsService.GetShipmentSubs(entity);

        if (eventUser == null)
            return Results.NotFound();

        await shipmentSubsService.RemoveSubsScribeShipment(eventUser);
        return Results.NoContent();
    }
}
