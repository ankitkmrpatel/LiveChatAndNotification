namespace NotificationBackendService.Services
{
    public interface ICurrentUserIdentity
    {
        string Email { get; init; }
        bool IsBlocked { get; init; }
        string Name { get; init; }
        Guid UserId { get; init; }
        string UserName { get; init; }
        Guid AccessCode { get; init; }
    }
}