using System.Text.Json.Serialization;

namespace Airdrops.GaiaChat.Scheduler.Domain.Dtos.Ocean
{
    public class Node
    {
        [JsonPropertyName("_index")]
        public string Index { get; set; }

        [JsonPropertyName("_id")]
        public string Id { get; set; }

        [JsonPropertyName("_score")]
        public object Score { get; set; } // Changed to object to accommodate null

        [JsonPropertyName("_source")]
        public Source Source { get; set; }

        [JsonPropertyName("sort")]
        public List<int> Sort { get; set; }
    }
}
