using Polly.Extensions.Http;
using Polly;
using Refit;
using Airdrops.Nodes.Infrastructure.Abstractions;
using Serilog;
using ILogger = Microsoft.Extensions.Logging.ILogger;
using Microsoft.Extensions.DependencyInjection;

namespace Airdrops.Nodes.Infrastructure
{
    public static class InfrastructureDependencies
    {
        public static IServiceCollection RegisterInfrastructure(this IServiceCollection services)
        {
            services
                .AddRefitClient<IOpenTdbApiClient>()
                .ConfigureHttpClient((ctx, c) =>
                {
                    var configuration = ctx.GetService<IConfiguration>();
                    var opentDbBaseUrl = configuration!["Endpoints:OpentdbBaseUrl"];

                    ArgumentException.ThrowIfNullOrWhiteSpace(opentDbBaseUrl, nameof(opentDbBaseUrl));

                    c.BaseAddress = new Uri(opentDbBaseUrl);
                    c.Timeout = TimeSpan.FromMinutes(2);
                })
                .AddPolicyHandler(GetPolicy);

            services
               .AddRefitClient<IGaiaChatApiClient>()
               .ConfigureHttpClient((ctx, c) =>
               {
                   var configuration = ctx.GetService<IConfiguration>();
                   var gaiaChatBaseUrl = configuration!["Endpoints:GaiaChatBaseUrl"];

                   ArgumentException.ThrowIfNullOrWhiteSpace(gaiaChatBaseUrl, nameof(gaiaChatBaseUrl));
                   c.BaseAddress = new Uri(gaiaChatBaseUrl);
                   c.Timeout = TimeSpan.FromMinutes(2);
               })
               .AddPolicyHandler(GetPolicy);

            return services;

            static IAsyncPolicy<HttpResponseMessage> GetPolicy(IServiceProvider serviceProvider, HttpRequestMessage requestMessage)
            {
                var logger = serviceProvider.GetRequiredService<ILogger>();
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(response => (int)response.StatusCode == 429 || (int)response.StatusCode > 500)
                    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), 
                    (outcome, timespan, retryAttempt, context) =>
                    {
                        logger.LogWarning("Retry {RetryAttempt} encountered an error. Waiting {Timespan} before next retry. Outcome: {StatusCode}", retryAttempt, timespan, outcome.Result.StatusCode);
                    });
            }
        }
    }
}
