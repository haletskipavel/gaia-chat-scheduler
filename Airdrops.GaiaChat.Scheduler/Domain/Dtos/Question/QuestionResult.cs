using System.Text.Json.Serialization;

namespace Airdrops.GaiaChat.Scheduler.Domain.Dtos.Question
{
    public class QuestionResult
    {
        [JsonPropertyName("question")]
        public string Question { get; set; }
    }
}
