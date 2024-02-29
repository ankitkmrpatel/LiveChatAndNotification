namespace NotificationBackendService.Models.Dto
{
    public class GetUserHubConnectionDto
    {
        public Guid UserId { get; set; }
        public string? UserAgent { get; set; }
        public string ConnectionId { get; set; }
        public bool IsConnected { get; set; }
    }
}
