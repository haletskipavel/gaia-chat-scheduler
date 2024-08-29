using Microsoft.Extensions.DependencyInjection;
using Polly.Extensions.Http;
using Polly;
using Refit;
using Airdrops.Nodes.Infrastructure.Abstractions;
using Microsoft.Extensions.Configuration;

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
                })
                .AddPolicyHandler(_ => GetPolicy());

            services
               .AddRefitClient<IGaiaChatApiClient>()
               .ConfigureHttpClient((ctx, c) =>
               {
                   var configuration = ctx.GetService<IConfiguration>();
                   var gaiaChatBaseUrl = configuration!["Endpoints:GaiaChatBaseUrl"];

                   ArgumentException.ThrowIfNullOrWhiteSpace(gaiaChatBaseUrl, nameof(gaiaChatBaseUrl));
                   c.BaseAddress = new Uri(gaiaChatBaseUrl);
               })
               .AddPolicyHandler(_ => GetPolicy());

            return services;

            static IAsyncPolicy<HttpResponseMessage> GetPolicy()
            {
                return HttpPolicyExtensions
                    .HandleTransientHttpError()
                    .OrResult(response => (int)response.StatusCode == 429 || (int)response.StatusCode > 500)
                    .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
            }
        }
    }
}
