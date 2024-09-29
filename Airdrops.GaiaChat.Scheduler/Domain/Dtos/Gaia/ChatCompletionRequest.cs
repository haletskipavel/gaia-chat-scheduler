using System.Text.Json.Serialization;

namespace Airdrops.GaiaChat.Scheduler.Domain.Dtos.Gaia
{
    public class ChatCompletionRequest
    {
        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }
    }
}
