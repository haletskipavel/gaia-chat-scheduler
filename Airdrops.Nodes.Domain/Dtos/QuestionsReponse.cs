using System.Text.Json.Serialization;

namespace Airdrops.Nodes.Domain.Dtos
{
    public class QuestionsReponse
    {
        [JsonPropertyName("results")]
        public List<QuestionResult> Results { get; set; }
    }
}
