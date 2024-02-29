using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationBackendService.Data.Entities;

public class UserEventSubsSetting : BaseEntity
{
    public UserEventSubsSetting()
    {
        this.SubsSettingsItems = new List<UserEventSubsSettingsItem>();
    }

    public Guid UserId { get; set; }
    public string EventModuleId { get; set; }
    public string EventModuleType { get; set; }

    public virtual List<UserEventSubsSettingsItem> SubsSettingsItems { get; set; }
}

public class UserEventSubsSettingsItem : BaseEntity
{
    public Guid UserEventSettingsId { get; set; }
    public string EventType { get; set; }
    public string EventItem { get; set; }
    public string EventSubsDeliveryMode { get; set; }
    public TimeSpan NotificationTimeout { get; set; }

    [ForeignKey("UserEventSettingsId")]
    public virtual UserEventSubsSetting UserEventSubsSetting { get; set; }
}
