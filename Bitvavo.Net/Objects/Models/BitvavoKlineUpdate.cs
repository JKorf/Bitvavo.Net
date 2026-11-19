using Bitvavo.Net.Enums;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Candlestick update for a symbol and interval
    /// </summary>
    public record BitvavoKlineUpdate
    {
        /// <summary>
        /// ["<c>market</c>"] Symbol
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>interval</c>"] Candlestick interval
        /// </summary>
        [JsonPropertyName("interval")]
        public KlineInterval Interval { get; set; }
        /// <summary>
        /// ["<c>candle</c>"] Updated candlesticks
        /// </summary>
        [JsonPropertyName("candle")]
        public BitvavoKline[] Klines { get; set; } = [];
    }
}
