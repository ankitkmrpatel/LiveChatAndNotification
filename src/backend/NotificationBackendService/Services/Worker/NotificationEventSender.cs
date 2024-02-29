using NotificationBackendService.Extentions;

namespace NotificationBackendService.Services.Worker;

public class NotificationEventSender : BackgroundService
{
    private int _executionCount;
    private readonly ILogger<NotificationEventSubscriber> _logger;
    private readonly IServiceScopeFactory scopeFactory;

    public NotificationEventSender(ILogger<NotificationEventSubscriber> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        this.scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted - Sender Service running.");

        // When the timer should have no due-time, then do the work once now.
        await DoWork();

        using PeriodicTimer timer = new(TimeSpan.FromMinutes(5));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                await DoWork();
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogError("Timed Hosted - Sender Service is stopping.");
        }
    }

    // Could also be a async method, that can be awaited in ExecuteAsync above
    private async Task DoWork()
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var userEventDelivery = scope.ServiceProvider.GetRequiredService<IUserEventDeliveryService>();

            //Read All Events
            var events = await userEventDelivery.GetAllUndeliveredEvents();

            if (events == null || events.Count == 0)
                return;

            //Send the Event To User Using Hub
            foreach (var evt in events)
            {
                if (evt == null)
                    continue;

#warning Add Method To Send Mail / SMS
                //Send Email
                //Send SMS

                evt.Emailed = true;
                evt.SMSSent = true;

                await userEventDelivery.UpdateUserEventDelivery(evt);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception at Processing Service : ", ex);
        }
        finally
        {
            int count = Interlocked.Increment(ref _executionCount);
            _logger.LogInformation("Timed Hosted - Sender Service is working. Count: {Count}", count);
        }
    }
}