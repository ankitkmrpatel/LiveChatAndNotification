using Microsoft.AspNetCore.Authorization;
using NotificationBackendService.Data.Entities;
using NotificationBackendService.Extentions;
using NotificationBackendService.Models.Dto;
using NotificationBackendService.Services;

namespace NotificationBackendService.Endpoints;

public static class EventEndpoints
{
    public static void MapEventEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapPost("api/events", CreateNewEvent)
            .WithName("CreateNewEvent").WithDisplayName("Create New Event Item")
            .Produces<EventItemDto>(StatusCodes.Status201Created)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapGet("api/events", GetAllEvents)
               .WithName("GetAllEvents").WithDisplayName("Get All Events")
               .Produces<IReadOnlyCollection<EventItemDto>>(StatusCodes.Status200OK)
               .Produces(StatusCodes.Status400BadRequest)
               .Produces(StatusCodes.Status500InternalServerError);
        
        app.MapGet("api/events/{id}", GetEventById)
            .WithName("GetEventById").WithDisplayName("Get Event Item")
            .Produces<EventItemDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
        
        app.MapPut("api/events/{id}", UpdateEvent)
            .WithName("UpdateEvent").WithDisplayName("Update Event Item")
            .Produces<EventItemDto>(StatusCodes.Status200OK)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);

        app.MapDelete("api/events/{id}", DeleteEvent)
            .WithName("DeleteEvent")
            .WithDisplayName("Delete Event Item")
            .Produces(StatusCodes.Status204NoContent)
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status404NotFound)
            .Produces(StatusCodes.Status500InternalServerError);
    }

    /// Create New Catalog Item
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="newItem"></param>
    /// <returns></returns>
    //[Authorize]
    private static async Task<IResult> CreateNewEvent(IEventService eventService, CreateEventItemDto newItem)
    {
        if (string.IsNullOrEmpty(newItem.EventDesctipion))
            return Results.BadRequest();
        
        if (string.IsNullOrEmpty(newItem.EventType))
            return Results.BadRequest();
        
        if (string.IsNullOrEmpty(newItem.EventModuleId))
            return Results.BadRequest();

        if (string.IsNullOrEmpty(newItem.EventModuleType))
            return Results.BadRequest();

        var entity = new Event()
        {
            EventModuleId = newItem.EventModuleId,
            EventModuleType = newItem.EventModuleType,
            EventType = newItem.EventType,
            EventDesctipion = newItem.EventDesctipion,
            EventData = newItem.EventData
        };

        await eventService.CreateAsync(entity);
        var item = entity.AsDto();

        return Results.Created($"api/events/{entity.Id}", item);
    }

    /// <summary>
    /// Get All Catalog Items
    /// </summary>
    /// <param name="repo"></param>
    /// <returns></returns>
    [Authorize]
    private static async Task<IResult> GetAllEvents(IEventService eventService)
    {
        var entities = await eventService.GetAllEventsAsync();
        var items = entities.Select(x => x.AsDto());

        return Results.Ok(items);
    }

    /// <summary>
    /// Get the Catalog Item
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    private static async Task<IResult> GetEventById(IEventService eventService, Guid id)
    {
        if (id.Equals(Guid.Empty))
        {
            return Results.BadRequest();
        }

        var entity = await eventService.GetEventyById(id);

        if (null == entity)
            return Results.NotFound();

        var item = entity.AsDto();
        return Results.Ok(item);
    }

    /// <summary>
    /// Update Catalog Item
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="id"></param>
    /// <param name="updateItem"></param>
    /// <returns></returns>
    [Authorize]
    private static async Task<IResult> UpdateEvent(IEventService eventService, Guid id, UpdateEventItemDto updateItem)
    {
        if (id.Equals(Guid.Empty))
        {
            return Results.BadRequest();
        }

        var existingItem = await eventService.GetEventyById(id);

        if (null == existingItem)
            return Results.NotFound();

        existingItem.EventType = updateItem.EventType;
        existingItem.EventModuleType = updateItem.EventModuleType;
        existingItem.EventModuleId = updateItem.EventModuleId;
        existingItem.EventDesctipion = updateItem.EventDesctipion;
        existingItem.EventData = updateItem.EventData;
        
        await eventService.UpdateEvent(existingItem);

        return Results.Ok(existingItem);
    }

    /// <summary>
    /// Delete The Catalog Item
    /// </summary>
    /// <param name="repo"></param>
    /// <param name="id"></param>
    /// <returns></returns>
    [Authorize]
    private static async Task<IResult> DeleteEvent(IEventService eventService, Guid id)
    {
        if (id.Equals(Guid.Empty))
        {
            return Results.BadRequest();
        }

        var existingItem = await eventService.GetEventyById(id);

        if (null == existingItem)
            return Results.NotFound();

        await eventService.DeleteEvent(existingItem.Id);

        return Results.NoContent();
    }
}
