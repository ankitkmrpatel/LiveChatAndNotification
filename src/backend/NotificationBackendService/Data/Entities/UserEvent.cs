using System.ComponentModel.DataAnnotations.Schema;

namespace NotificationBackendService.Data.Entities;

public class UserEvent : BaseEntity
{
    /// <summary>
    /// User Id 
    /// </summary>
    public Guid UserId { get; set; }

    /// <summary>
    /// Event Id From Event Table
    /// </summary>
    public Guid EventId { get; set; }

    public Guid UserEventReadId { get; set; }


    [ForeignKey("UserEventReadId")]
    public UserEventReadStatus EventReadStatus { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; }

    [ForeignKey("EventId")]
    public virtual Event Event { get; set; }
}
