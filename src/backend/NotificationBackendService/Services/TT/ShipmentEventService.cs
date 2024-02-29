using NotificationBackendService.Data;
using NotificationBackendService.Data.Entities;

namespace NotificationBackendService.Services;

public class ShipmentEventService : IShipmentEventService
{
    private readonly IRepo repo;
    private readonly IUserService userService;
    private readonly ICurrentUserIdentity currentUser;

    public ShipmentEventService(IRepo repo, IUserService userService, ICurrentUserIdentity currentUser)
    {
        this.repo = repo;
        this.userService = userService;
        this.currentUser = currentUser;
    }

    public async Task<IReadOnlyCollection<Event>> GetShipmentEvents()
    {
        var user = await GetUserOrgCodes();
        _ = user ?? throw new ArgumentException(nameof(user));

        var shipmentEventsQuery = $@"WITH vars as (WITH split(word, csv) AS (
                  -- 'initial query' (see SQLite docs linked above)
                  SELECT 
                    -- in final WHERE, we filter raw csv (1st row) and terminal ',' (last row)
                    '', 
                    -- here you can SELECT FROM e.g. another table: col_name||',' FROM X
                    '{user.Organizations}' ||',' -- terminate with ',' indicating csv ending
                  -- 'recursive query'
                  UNION ALL SELECT
                    substr(csv, 0, instr(csv, ',')), -- each word contains text up to next ','
                    substr(csv, instr(csv, ',') + 1) -- next recursion parses csv after this ','
                  FROM split -- recurse
                  WHERE csv != '' -- break recursion once no more csv words exist
                ) SELECT word FROM split 
                WHERE word !='' -- filter out 1st/last rows
                )

                --SELECT vars.word FROM vars

                Select e.Id,
                       e.EventType,
                       e.EventModuleId,
                       e.EventModuleType,
                       e.EventDesctipion,
                       e.EventData 
                   From Events e
                   Join Shipments s 
                      on e.EventModuleId = 'SHP' and e.EventModuleType = s.ShipmentId
                   Where s.CneeCode in (SELECT vars.word FROM vars) 
                      or s.CnorCode in (SELECT vars.word FROM vars)
                      or s.NotifCode in (SELECT vars.word FROM vars)";

        var result = await repo.ExecuteQueryAsync(shipmentEventsQuery,
            x => new Event()
            {
                Id = Guid.Parse(x[0].ToString() ?? throw new Exception()),
                EventType = (string)x[1],
                EventModuleId = (string)x[2],
                EventModuleType = (string)x[3],
                EventDesctipion = (string)x[4],
                EventData = Convert.ToDateTime(x[5])
            });

        return result;
    }

    private async Task<User?> GetUserOrgCodes()
    {
        return await userService.GetUserAsync(currentUser.UserName);
    }
}
