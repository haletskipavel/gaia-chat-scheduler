using System.Text.Json.Serialization;

namespace Airdrops.Nodes.Domain.Dtos
{
    public class QuestionResult
    {
        [JsonPropertyName("question")]
        public string Question { get; set; }
    }
}
