using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using TimerApi;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddSingleton<ITimerGrain,TimerGrain>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Host.UseOrleans(builder =>
                builder.UseLocalhostClustering()
                .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(TimerGrain).Assembly).WithReferences())
                .UseDashboard(options => { })
                .Configure<SiloMessagingOptions>(options =>
                {                   
                    options.ResponseTimeout = TimeSpan.FromSeconds(30 * 60); // was 10
                    options.ResponseTimeoutWithDebugger = TimeSpan.FromSeconds(30 * 60); // was 10
                })
            );

var app = builder.Build();
app.Map("/dashboard", x => x.UseOrleansDashboard());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseAuthorization();

app.MapControllers();

app.Run();
