namespace NotificationBackendService.Data.Entities;

public class User : BaseEntity
{
    public string Name { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public bool EmailConfirmed { get; set; }
    public string Organizations { get; set; }
    public bool IsBlocked { get; set; } = false;
}
