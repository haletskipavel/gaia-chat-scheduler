using System.Text.Json.Serialization;

namespace Airdrops.GaiaChat.Scheduler.Domain.Dtos.Telegram
{
    public class SendMessageRequest
    {
        [JsonPropertyName("chat_id")]
        public long ChatId { get; set; }

        [JsonPropertyName("text")]
        public string Text { get; set; }

        [JsonPropertyName("parse_mode")]
        public string ParseMode { get; set; }
    }
}
