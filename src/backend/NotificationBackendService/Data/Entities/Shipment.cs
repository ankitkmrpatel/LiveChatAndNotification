namespace NotificationBackendService.Data.Entities;

public class Shipment : BaseEntity
{
    public string ShipmentId { get; set; }

    public string? CneeCode { get; set; }
    public string? CneeName { get; set; }
    public string? CnorCode { get; set; }
    public string? CnorName { get; set; }
    public string? NotifCode { get; set; }
    public string? NotifName { get; set; }

    public string? GoodsDesc { get; set; }

    public string? POLCode { get; set; }
    public string? POLName { get; set; }
    public string? PODCode { get; set; }
    public string? PODName { get; set; }
}
