using Airdrops.Nodes.Scheduler.Jobs;
using Quartz;
using Serilog;
using Airdrops.Nodes.Infrastructure;
using Airdrops.GaiaChat.Scheduler.Jobs;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console(
                outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj} {NewLine}{Exception}")
            .CreateLogger();

        builder.Logging.ClearProviders();
        builder.Logging.AddSerilog();

        builder.Services.AddQuartz(q =>
        {
            q.UseSimpleTypeLoader();
            q.UseInMemoryStore();
            var disableGaiaChatJob = Environment.GetEnvironmentVariable("DISABLE_GAIA_CHAT_JOB") == "true";
            if (!disableGaiaChatJob)
            {
                q.ScheduleJob<GaiaChatJob>(trigger => trigger
                .WithIdentity(nameof(GaiaChatJob))
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(new Random().Next(30, 60))
                    .RepeatForever())
                );
            }
            var disableOceanEligibilityJob = Environment.GetEnvironmentVariable("DISABLE_OCEAN_ELIGIBILITY_CHECKER_JOB") == "true";
            if (!disableOceanEligibilityJob)
            {
                q.ScheduleJob<OceanEligibilityCheckerJob>(trigger => trigger
               .WithIdentity(nameof(OceanEligibilityCheckerJob))
               .StartNow()
               .WithSimpleSchedule(x => x
                   .WithIntervalInMinutes(60)
                   .RepeatForever())
                );
            }
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
    }
}