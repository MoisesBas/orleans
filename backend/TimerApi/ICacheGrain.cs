using Orleans;
using Orleans.Concurrency;
namespace TimerApi;
public interface ICacheGrain<T> : IGrainWithStringKey
{
    Task Set(Immutable<T> item, TimeSpan timeToKeep);
    Task<Immutable<T>> Get();
    Task Clear();
    Task Refresh();
}