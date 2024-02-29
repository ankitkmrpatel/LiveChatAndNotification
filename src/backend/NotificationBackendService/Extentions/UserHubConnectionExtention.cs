using NotificationBackendService.Data.Entities;
using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Extentions
{
    public static class UserHubConnectionExtention
    {
        public static GetUserHubConnectionDto AsDto(this UserHubConnection userHub)
        {
            return new GetUserHubConnectionDto()
            {
                ConnectionId = userHub.ConnectionId,
                IsConnected = userHub.IsConnected,
                UserId = userHub.UserId,
                UserAgent = userHub.UserAgent
            };
        }
    }
}
