using Microsoft.AspNetCore.Mvc;
using Orleans;

namespace TimerApi.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly ITimerGrain _grain;
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger, ITimerGrain grain)
    {
        _logger = logger;
        _grain = grain;

    }

    [HttpGet("GetWeatherForecast")]
    public IEnumerable<WeatherForecast> GetWeatherForecast()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }


    [HttpGet("GetTimer")]
    public async Task<IActionResult> GetTimer()
    {
        var timerValue = await _grain.GetTimerValue();
        return Ok(timerValue);
    }

    [HttpPost("SetTimer")]
    public async Task<IActionResult> SetTimer([FromBody] TimerRequest timerRequest)
    {
        var dueTime = TimeSpan.FromSeconds(timerRequest.DueTimeInSeconds);
        var period = TimeSpan.FromSeconds(timerRequest.PeriodInSeconds);
        await _grain.SetTimer(dueTime, period);
        return Ok();
    }

}
