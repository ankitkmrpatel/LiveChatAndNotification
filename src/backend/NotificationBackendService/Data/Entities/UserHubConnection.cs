namespace NotificationBackendService.Data.Entities
{
    public class UserHubConnection : BaseEntity
    {
        public Guid UserId { get; set; }
        public string ConnectionId { get; set; }
        public string? UserAgent { get; set; }
        public bool IsConnected { get; set; }
    }
}
