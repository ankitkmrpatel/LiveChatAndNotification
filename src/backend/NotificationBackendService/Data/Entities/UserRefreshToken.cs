namespace NotificationBackendService.Data.Entities;

public class UserRefreshToken : BaseEntity
{
    public Guid UserId { get; set; }
    public string TokenHash { get; set; }
    public string TokenSalt { get; set; }
    public DateTime TS { get; set; }
    public DateTime ExpiryDate { get; set; }
}
