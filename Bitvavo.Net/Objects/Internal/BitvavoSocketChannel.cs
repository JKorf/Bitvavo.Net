using Bitvavo.Net.Enums;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Internal
{
    internal class BitvavoSocketChannel
    {
        [JsonPropertyName("name")]
        public string Name { get; }

        [JsonPropertyName("markets")]
        public object Markets { get; }

        [JsonPropertyName("interval")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public KlineInterval[]? Intervals { get; }

        public BitvavoSocketChannel(string name, object markets, KlineInterval[]? intervals = null)
        {
            Name = name;
            Markets = markets;
            Intervals = intervals;
        }

        public static string GetTopicFilter(string symbol, KlineInterval interval) => $"{symbol}:{(int)interval}";
    }
}
