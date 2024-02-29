using NotificationBackendService.Data.Entities;
using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Extentions
{
    public static class EventDtoExtention
    {
        public static EventItemDto AsDto(this Event item)
        {
            return new EventItemDto() 
            {
                EventData = item.EventData,
                EventDesctipion = item.EventDesctipion,
                EventId = item.Id,
                EventModuleId = item.EventModuleId,
                EventModuleType = item.EventModuleType,
                EventType = item.EventType
            };
        }
    }
}
