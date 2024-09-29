using System.Text.Json.Serialization;

namespace Airdrops.GaiaChat.Scheduler.Domain.Dtos.Gaia
{
    public class Choice
    {
        [JsonPropertyName("message")]
        public Message Message { get; set; }
    }
}
