using System.Text.Json.Serialization;

namespace Airdrops.Nodes.Domain.Dtos
{
    public class ChatCompletionRequest
    {
        [JsonPropertyName("messages")]
        public List<Message> Messages { get; set; }
    }
}
