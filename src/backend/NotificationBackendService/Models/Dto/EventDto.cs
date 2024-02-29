namespace NotificationBackendService.Models.Dto;

public class CreateEventItemDto
{
    public string EventType { get; set; }
    public string EventModuleId { get; set; }
    public string EventModuleType { get; set; }
    public string EventDesctipion { get; set; }
    //public Dictionary<string, DateTime> EventData { get; set; }
    public DateTime EventData { get; set; }
}

public class UpdateEventItemDto : CreateEventItemDto
{
    public Guid Id { get; set; }
}

public class EventItemDto
{
    public Guid EventId { get; set; }
    public string EventType { get; set; }
    public string EventModuleId { get; set; }
    public string EventModuleType { get; set; }
    public string EventDesctipion { get; set; }
    //public Dictionary<string, DateTime> EventData { get; set; }
    public DateTime EventData { get; set; }
}
