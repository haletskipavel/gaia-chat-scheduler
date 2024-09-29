using Airdrops.GaiaChat.Scheduler.Domain.Dtos.Telegram;
using Refit;

namespace Airdrops.Nodes.Infrastructure.Abstractions
{
    public interface ITelegramBotApiClient
    {
        [Get("/bot{botToken}/sendMessage")]
        [Headers("accept: application/json", "Content-Type: application/json")]
        Task SendMessageAsync(string botToken, [Body]SendMessageRequest request);
    }
}
