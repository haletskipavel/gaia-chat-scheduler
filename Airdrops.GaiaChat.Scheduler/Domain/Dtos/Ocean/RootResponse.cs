using System.Text.Json.Serialization;

namespace Airdrops.GaiaChat.Scheduler.Domain.Dtos.Ocean
{
    public class RootResponse
    {
        [JsonPropertyName("nodes")]
        public List<Node> Nodes { get; set; }
    }
}
