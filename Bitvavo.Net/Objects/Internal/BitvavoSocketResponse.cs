using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Internal
{
    internal class BitvavoSocketResponse<T>
    {
        [JsonPropertyName("response")]
        public T? Response { get; set; }

        [JsonPropertyName("errorCode")]
        public int? ErrorCode { get; set; }

        [JsonPropertyName("error")]
        public string? Error { get; set; }
    }
}
