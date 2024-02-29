namespace NotificationBackendService.Data.Entities;

public class UserEventDeliveryStatus : BaseEntity
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public bool EventRead { get; set; }
    public bool DeliveredStatus { get; set; }
    public string DeliverMode { get; set; }
    public string DeliverTarget { get; set; }
    public DateTime DeliverTime { get; set; }

}
