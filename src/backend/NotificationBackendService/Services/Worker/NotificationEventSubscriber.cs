using NotificationBackendService.Extentions;

namespace NotificationBackendService.Services.Worker;

public class NotificationEventSubscriber : BackgroundService
{
    private int _executionCount;
    private readonly ILogger<NotificationEventSubscriber> _logger;
    private readonly IServiceScopeFactory scopeFactory;

    public NotificationEventSubscriber(ILogger<NotificationEventSubscriber> logger,
        IServiceScopeFactory scopeFactory)
    {
        _logger = logger;
        this.scopeFactory = scopeFactory;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Timed Hosted Service running.");

        // When the timer should have no due-time, then do the work once now.
        await DoWork();

        using PeriodicTimer timer = new(TimeSpan.FromSeconds(10));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                await Task.Delay(TimeSpan.FromSeconds(10), stoppingToken);
                await DoWork();

                int count = Interlocked.Increment(ref _executionCount);
                _logger.LogInformation("Timed Hosted Service is working. Count: {Count}", count);
            }
        }
        catch (OperationCanceledException)
        {
            _logger.LogError("Timed Hosted Service is stopping.");
        }
    }

    // Could also be a async method, that can be awaited in ExecuteAsync above
    private async Task DoWork()
    {
        await SendNewEventToUser();
        await SendUserEvent();
    }

    private async Task SendNewEventToUser()
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var eventService = scope.ServiceProvider.GetRequiredService<IEventService>();
            var eventUserSubsWorker = scope.ServiceProvider.GetRequiredService<INotificationSubsWorker>();

            //Read All Events
            var events = await eventService.GetAllUndeliveredEvents();

            if (events == null || events.Count == 0)
                return;

            //Send the Event To User Using Hub
            foreach (var evt in events)
            {
                try
                {
                    await eventUserSubsWorker.Process(evt.AsDto());

                    evt.IsDevliverd = true;
                    await eventService.UpdateEvent(evt);
                }
                catch (Exception ex)
                {
                    //Log the Details for Event Sending
                    System.Diagnostics.Debug.WriteLine("Exception at Processing Events", ex);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception at Processing Service : ", ex);
        }
    }

    private async Task SendUserEvent()
    {
        try
        {
            using var scope = scopeFactory.CreateScope();
            var eventService = scope.ServiceProvider.GetRequiredService<IEventService>();
            var eventUserSubsWorker = scope.ServiceProvider.GetRequiredService<INotificationSubsWorker>();

            //Read All Events
            var events = await eventService.GetAllUndeliveredEvents();

            if (events == null || events.Count == 0)
                return;

            //Send the Event To User Using Hub
            foreach (var evt in events)
            {
                try
                {
                    await eventUserSubsWorker.Process(evt.AsDto());

                    evt.IsDevliverd = true;
                    await eventService.UpdateEvent(evt);
                }
                catch (Exception ex)
                {
                    //Log the Details for Event Sending
                    System.Diagnostics.Debug.WriteLine("Exception at Processing Events", ex);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError("Exception at Processing Service : ", ex);
        }
    }
}