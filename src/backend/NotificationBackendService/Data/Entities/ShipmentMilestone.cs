namespace NotificationBackendService.Data.Entities;

public class ShipmentMilestone : BaseEntity
{
    public string ShipmentId { get; set; }
    public int Sequence { get; set; }
    public string Description { get; set; }
    public string EventCode { get; set; }
    public string ActualDate { get; set; }
    public string EstimatedDate { get; set; }

}
