namespace NotificationBackendService.Data.Entities;

public class Event : BaseEntity
{
    public string EventType { get; set; }
    public string EventModuleId { get; set; }
    public string EventModuleType { get; set; }
    public string EventDesctipion { get; set; }
    //public Dictionary<string, DateTime> EventData { get; set; }
    public DateTime EventData { get; set; }
    public bool IsDevliverd { get; set; }
}