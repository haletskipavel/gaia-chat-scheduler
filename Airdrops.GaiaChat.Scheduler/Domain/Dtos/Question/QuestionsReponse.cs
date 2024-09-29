using System.Text.Json.Serialization;

namespace Airdrops.GaiaChat.Scheduler.Domain.Dtos.Question
{
    public class QuestionsReponse
    {
        [JsonPropertyName("results")]
        public List<QuestionResult> Results { get; set; }
    }
}
