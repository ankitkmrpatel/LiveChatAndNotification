using NotificationBackendService.Data.Entities;
using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Extentions
{
    public static class ShipmentDtoExtention
    {
        public static GetShipmentDto AsDto(this Shipment item, IReadOnlyCollection<ShipmentMilestone>? milestones = null)
        {
            return new GetShipmentDto()
            {
                ShipmentId = item.ShipmentId,
                NotifCode = item.NotifCode,
                CneeCode = item.CneeCode,
                CneeName = item.CneeName,
                CnorCode = item.CnorCode,
                CnorName = item.CnorName,
                GoodsDesc = item.GoodsDesc,
                NotifName = item.NotifName,
                PODCode = item.PODCode,
                PODName = item.PODName,
                POLCode = item.POLCode,
                POLName = item.POLName,

                Milestones = milestones?.Select(x => x.AsDto())?
                    .ToList()
            };
        }

        public static ShipmentMilestoneDto AsDto(this ShipmentMilestone milestone)
        {
            return new ShipmentMilestoneDto()
            {
                ShipmentId = milestone.ShipmentId,
                Sequence = milestone.Sequence,
                Description = milestone.Description,
                EventCode = milestone.EventCode,
                EstimatedDate = milestone.EstimatedDate,
                ActualDate = milestone.ActualDate,
            };
        }
    }
}
