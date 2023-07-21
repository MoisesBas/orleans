using Orleans;
namespace TimerApi;
public class TimerGrain : Grain, ITimerGrain
{
    private readonly ILogger<TimerGrain> logger;
    private IDisposable timer;
    private DateTime lastTimerValue = DateTime.MinValue;

    public TimerGrain(ILogger<TimerGrain> logger)
    {
        this.logger = logger;
    }

    public override async Task OnActivateAsync()
    {
        await SetTimer(TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(1));
    }

    public Task<string> GetTimerValue()
    {
        return Task.FromResult(lastTimerValue.ToString("HH:mm:ss"));
    }

    public Task SetTimer(TimeSpan dueTime, TimeSpan period)
    {
        // Clear any existing timer
        timer?.Dispose();

        // Set a new timer
        timer = RegisterTimer(HandleTimer, null, dueTime, period);

        return Task.CompletedTask;
    }

    private Task HandleTimer(object state)
    {
        lastTimerValue = DateTime.Now;
        logger.LogInformation($"Timer value updated: {lastTimerValue}");
        return Task.CompletedTask;
    }
}