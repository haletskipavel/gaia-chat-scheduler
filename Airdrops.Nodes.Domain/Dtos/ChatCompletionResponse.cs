using System.Text.Json.Serialization;

namespace Airdrops.Nodes.Domain.Dtos
{
    public class ChatCompletionResponse
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("object")]
        public string Object { get; set; }

        [JsonPropertyName("created")]
        public long Created { get; set; }

        [JsonPropertyName("choices")]
        public List<Choice> Choices { get; set; }
    }
}
