using NotificationBackendService.Data.Entities;
using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Extentions
{
    public static class UserDtoExtention
    {
        public static GetUserTokenDto AsDto(this User item)
        {
            return new GetUserTokenDto() 
            {
                Name = item.Name,
                Username = item.UserName,
                Email = item.Email, 
                Organizations = item.Organizations,
            };
        }
    }
}
