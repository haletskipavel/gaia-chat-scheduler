using System.Text.Json.Serialization;

namespace Airdrops.GaiaChat.Scheduler.Domain.Dtos.Ocean
{
    public class Source
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("eligible")]
        public bool Eligible { get; set; }

        [JsonPropertyName("uptime")]
        public double Uptime { get; set; }

        [JsonPropertyName("lastCheck")]
        public long LastCheck { get; set; }

        [JsonPropertyName("address")]
        public string Address { get; set; }

        [JsonPropertyName("allowedAdmins")]
        public List<string> AllowedAdmins { get; set; }

        [JsonPropertyName("codeHash")]
        public string CodeHash { get; set; }

        [JsonPropertyName("http")]
        public bool Http { get; set; }

        [JsonPropertyName("p2p")]
        public bool P2P { get; set; }

        [JsonPropertyName("publicKey")]
        public string PublicKey { get; set; }

        [JsonPropertyName("version")]
        public string Version { get; set; }
    }
}
