using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Internal
{
    internal class BitvavoSocketEventResponse
    {
        [JsonPropertyName("event")]
        public string? Event { get; set; }

        [JsonPropertyName("errorCode")]
        public int? ErrorCode { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
