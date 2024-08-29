using System.Text.Json.Serialization;

namespace Airdrops.Nodes.Domain.Dtos
{
    public class Choice
    {
        [JsonPropertyName("message")]
        public Message Message { get; set; }
    }
}
