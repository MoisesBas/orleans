using Orleans;
namespace TimerApi;
public interface ITimerGrain : IGrainWithGuidKey
{
    Task<string> GetTimerValue();
    Task SetTimer(TimeSpan dueTime, TimeSpan period);
}