using Bitvavo.Net.Enums;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Internal
{
    internal class BitvavoSocketChannel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("markets")]
        public string[] Symbols { get; set; } = [];

        [JsonPropertyName("interval")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public KlineInterval[]? Intervals { get; set; }

        public static string GetTopicFilter(string symbol, KlineInterval interval) => $"{symbol}:{(int)interval}";
    }
}
