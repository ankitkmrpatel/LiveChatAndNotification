using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;
using NotificationBackendService.Models.Dto;

namespace NotificationBackendService.Services;

public interface IUserNotificationService
{
    Task CreateNewEventForUser(User user, EventItemDto eventItem);
    Task<IReadOnlyCollection<User>> GetApplicableUsers(Guid eventId);
    Task ValidateUserSubscription(User user, EventItemDto eventItem);
}

public class UserNotificationService : IUserNotificationService
{
    private readonly IRepo repo;
    private readonly IUserEventService userEventService;

    public UserNotificationService(IRepo repo, IUserEventService userEventService)
    {
        this.repo = repo;
        this.userEventService = userEventService;
    }

    public async Task CreateNewEventForUser(User user, EventItemDto eventItem)
    {
        var userEvent = await userEventService.GetUserEventById(user.Id, eventItem.EventId);

        if (userEvent == null)
        {
            //Create New User Event
            //Create New Read Status
            await userEventService.SaveNewUserEvent(new UserEvent()
            {
                EventId = eventItem.EventId,
                UserId = user.Id,
            });
        }
    }

    public async Task<IReadOnlyCollection<User>> GetApplicableUsers(Guid eventId)
    {
        var eventUsersQuery = $@"SELECT u.Id,
                            u.Name,
                            u.UserName,
                            u.Email,
                            u.Organizations
                    FROM Users u
                    INNER JOIN Shipments s ON u.Organizations IN (s.CneeCode, s.CnorCode, s.NotifCode)
                    LEFT JOIN Events e ON e.EventModuleId = s.ShipmentId
                    WHERE e.Id = '{eventId.ToString().ToUpper()}'
                        AND (
                            (SELECT Count(*)
                            FROM UserEventSubsSettings us
                            INNER JOIN UserEventSubsSettingsItems usi ON usi.UserEventSettingsId = us.Id
                            WHERE us.UserId = u.Id
                                AND us.EventModuleType = 'SHP'
                                AND us.EventModuleId = s.ShipmentId) > 0
                            AND e.EventDesctipion IN 
                                (SELECT usi.EventItem
                                FROM UserEventSubsSettings us
                                INNER JOIN UserEventSubsSettingsItems usi ON usi.UserEventSettingsId = us.Id
                                WHERE us.UserId = u.Id
                                    AND us.EventModuleType = 'SHP'
                                    AND us.EventModuleId = s.ShipmentId)
                            OR
                            (SELECT Count(*)
                            FROM UserEventSubsSettings us
                            INNER JOIN UserEventSubsSettingsItems usi ON usi.UserEventSettingsId = us.Id
                            WHERE us.UserId = u.Id
                                AND us.EventModuleType = 'SHP'
                                AND us.EventModuleId = s.ShipmentId) = 0
                            AND e.EventDesctipion IN 
                                (SELECT usi.EventItem
                                FROM UserEventSubsSettings us
                                INNER JOIN UserEventSubsSettingsItems usi ON usi.UserEventSettingsId = us.Id
                                WHERE us.UserId = u.Id
                                    AND us.EventModuleType = 'SHP'
                                    AND us.EventModuleId = '*')
                        );";  //Combine Other Module - Order/Transaction/RMS etc

        var result = await repo.ExecuteQueryAsync(eventUsersQuery,
            x => new User()
            {
                Id = Guid.Parse(x[0].ToString() ?? throw new Exception()),
                Name = (string)x[1],
                UserName = (string)x[2],
                Email = (string)x[3],
                Organizations = (string)x[4],
            });

        return result;
    }

    public Task ValidateUserSubscription(User user, EventItemDto eventItem)
    {
        throw new NotImplementedException();
    }
}
