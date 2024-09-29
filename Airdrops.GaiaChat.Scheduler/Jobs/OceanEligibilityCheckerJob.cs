using Airdrops.GaiaChat.Scheduler.Domain.Dtos.Telegram;
using Airdrops.Nodes.Infrastructure.Abstractions;
using Quartz;

namespace Airdrops.GaiaChat.Scheduler.Jobs
{
    public class OceanEligibilityCheckerJob : IJob
    {
        private readonly IOceanProtocolApiClient _oceanProtocolApiClient;
        private readonly ITelegramBotApiClient _telegramBotApiClient;
        private readonly IConfiguration _configuration;
        private readonly ILogger<OceanEligibilityCheckerJob> _logger;

        public OceanEligibilityCheckerJob(
            IOceanProtocolApiClient oceanProtocolApiClient,
            ITelegramBotApiClient telegramBotApiClient,
            IConfiguration configuration,
            ILogger<OceanEligibilityCheckerJob> logger)
        {
            _oceanProtocolApiClient = oceanProtocolApiClient;
            _telegramBotApiClient = telegramBotApiClient;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task Execute(IJobExecutionContext context)
        {
            _logger.LogInformation("Starting job. Job = {Job}", nameof(OceanEligibilityCheckerJob));

            try
            {
                var nodeId = _configuration["NodeId"];

                if (string.IsNullOrEmpty(nodeId))
                {
                    _logger.LogWarning("NodeId is null. Please set it as an environment variable. NodeId = {NodeId}", nodeId);
                    return;
                }

                var nodeResponse = await _oceanProtocolApiClient.GetNodesAsync(nodeId);
                var node = nodeResponse.Nodes.FirstOrDefault();

                if (node is null)
                {
                    _logger.LogWarning("Node is null. NodeId = {NodeId}", nodeId);
                    return;
                }

                var eligible = node.Source.Eligible == true;
                var message = CreateFormattedMessage(node.Source.Uptime, eligible, nodeId);

                var botToken = _configuration["BotToken"];

                if (string.IsNullOrEmpty(botToken))
                {
                    _logger.LogWarning("BotToken is null. Please set it as an environment variable. NodeId = {NodeId}", nodeId);
                    return;
                }

                await _telegramBotApiClient.SendMessageAsync(botToken, new SendMessageRequest
                {
                    ChatId = 555534760,
                    Text = message,
                    ParseMode = "MarkdownV2"
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during job execution. Job = {Job}", nameof(OceanEligibilityCheckerJob));
            }
            finally
            {
                _logger.LogInformation("Job execution finished. Job = {Job}", nameof(OceanEligibilityCheckerJob));
            }
        }

        private static (int days, int hours, int minutes) CalculateDaysAndMinutes(double totalSeconds)
        {
            var days = (int)(totalSeconds / (24 * 3600));
            var remainingSeconds = totalSeconds % (24 * 3600);
            var hours = (int)(remainingSeconds / 3600);
            remainingSeconds = remainingSeconds % 3600;
            var minutes =  (int)(remainingSeconds / 60);
            return (days, hours, minutes);
        }

        private string CreateFormattedMessage(double uptime, bool eligible, string nodeId)
        {
            var (days, hours, minutes) = CalculateDaysAndMinutes(uptime);
            var eligilbleIcon = "✅";
            var eligibleStatus = "Yes";
            if (!eligible)
            {
                eligilbleIcon = "❌";
                eligibleStatus = "No";
            }
            var currentDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm").Replace("-", "\\-").Replace(":", "\\:");

            string message = $"""
                *🔔 Ocean Node Status Update*

                📅 *Date:* {currentDateTime}
                📌 *Node ID:* {nodeId[..36]}\.\.\.
                🕒 *Uptime:* {days} days {hours} hours and {minutes} minutes
                {eligilbleIcon} *Eligible:* {eligibleStatus}
            """;

            return message;
        }
    }
}
