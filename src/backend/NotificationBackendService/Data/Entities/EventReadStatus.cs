using System;

namespace NotificationBackendService.Data.Entities;

public class UserEventReadStatus : BaseEntity
{
    public Guid EventId { get; set; }
    public Guid UserId { get; set; }
    public bool NotificationSent { get; set; }
    public DateTime NotificationTOut { get; set; }
    public bool NotificationRead { get; set; }
    public bool Emailed { get; set; }
    public bool SMSSent { get; set; }
}
