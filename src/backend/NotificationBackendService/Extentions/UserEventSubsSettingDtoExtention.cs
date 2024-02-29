using NotificationBackendService.Data.Entities;
using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Extentions
{
    public static class UserEventSubsSettingDtoExtention
    {
        public static GetShipmentSubsDto AsDto(this UserEventSubsSetting item)
        {
            return new GetShipmentSubsDto() 
            {
                EventUserId = item.Id,
                EventModuleId = item.EventModuleId,
                EventModuleType = item.EventModuleType,
                UserId = item.UserId
            };
        }
    }
}
