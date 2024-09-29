using Airdrops.GaiaChat.Scheduler.Domain.Dtos.Gaia;
using Airdrops.GaiaChat.Scheduler.Jobs;
using Airdrops.Nodes.Infrastructure.Abstractions;

using Quartz;

namespace Airdrops.Nodes.Scheduler.Jobs
{
    public class GaiaChatJob : IJob
    {
        private readonly ILogger<GaiaChatJob> _logger;
        private readonly IOpenTdbApiClient _openTdbApiClient;
        private readonly IGaiaChatApiClient _gaiaChatApiClient;

        public GaiaChatJob(
            ILogger<GaiaChatJob> logger,
            IOpenTdbApiClient openTdbApiClient,
            IGaiaChatApiClient gaiaChatApiClient)
        {
            _logger = logger;
            _openTdbApiClient = openTdbApiClient;
            _gaiaChatApiClient = gaiaChatApiClient;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("GaiaChatJob started. Job = {Job}", nameof(GaiaChatJob));

            try
            {
                var questionResponse = await _openTdbApiClient.GetQuestions(1);
                if (questionResponse == null || !questionResponse.Results.Any())
                {
                    _logger.LogWarning("No questions were retrieved from OpenTdb API.");
                    return;
                }

                var question = questionResponse.Results.FirstOrDefault()?.Question;

                if (string.IsNullOrEmpty(question))
                {
                    _logger.LogWarning("The retrieved question is null or empty.");
                    return;
                }

                await SendQuestionToGaiaChat(question);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing GaiaChatJob.");
            }
            finally
            {
                _logger.LogInformation("GaiaChatJob completed. Job = {Job}", nameof(GaiaChatJob));
            }
        }

        private async Task SendQuestionToGaiaChat(string question)
        {
            var chatCompletionRequest = new ChatCompletionRequest
            {
                Messages =
                [
                    new()
                    {
                        Role = "system",
                        Content = "You are a helpful assistant."
                    },
                    new() {
                        Role = "user",
                        Content = question
                    }
                ]
            };

            _logger.LogInformation("Sending request to gaia chat. Question: {Question}", question);

            await _gaiaChatApiClient.GetChatCompletionAsync(chatCompletionRequest);
        }
    }
}
