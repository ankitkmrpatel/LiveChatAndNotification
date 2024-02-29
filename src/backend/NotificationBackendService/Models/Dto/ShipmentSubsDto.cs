namespace NotificationBackendService.Models.Dto;

public class CreateNewShipmentSubsDto
{
    public string ShipmentId { get; set; }
    public virtual List<UserEventSubsSettingsItemDto> SubsSettings { get; set; }
}

public class UserEventSubsSettingsItemDto
{
    public string EventType { get; set; }
    public string EventItem { get; set; }
    public TimeSpan EventTimeout { get; set; } = TimeSpan.FromMinutes(15);
}

public class GetShipmentSubsDto
{
    public Guid EventUserId { get; set; }
    public string EventModuleId { get; set; }
    public string EventModuleType { get; set; }
    public Guid UserId { get; set; }
}
