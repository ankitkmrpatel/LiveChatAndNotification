using System.ComponentModel.DataAnnotations;

namespace NotificationBackendService.Data;

public class BaseEntity : IMustHaveId
{
    [Key]
    [Required]
    public Guid Id { get; set; }
    public DateTime CreatedTime { get; set; } = DateTime.Now;
}
