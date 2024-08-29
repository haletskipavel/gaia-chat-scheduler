using Airdrops.Nodes.Scheduler.Jobs;
using Quartz;
using Serilog;
using Airdrops.Nodes.Infrastructure;

var builder = Host.CreateApplicationBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Information()
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}")
    .CreateLogger();

builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

// Add services to the container
builder.Services.AddQuartz(q =>
{
    q.UseSimpleTypeLoader();
    q.UseInMemoryStore();

    // Register the job
    q.ScheduleJob<GaiaChatJob>(trigger => trigger
        .WithIdentity(nameof(GaiaChatJob))
        .StartNow()
        .WithSimpleSchedule(x => x
            .WithIntervalInHours(1)
            .RepeatForever())
    );
});

builder.Services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
builder.Services.RegisterInfrastructure();

var app = builder.Build();

var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();

lifetime.ApplicationStarted.Register(() =>
{
    Log.Logger.Information("Application has started.");
});

lifetime.ApplicationStopped.Register(() =>
{
    Log.Logger.Information("Application has stopped.");
});

app.Run();
