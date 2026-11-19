using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Trade update
    /// </summary>
    public record BitvavoTradeUpdate : BitvavoTrade
    {
        /// <summary>
        /// ["<c>market</c>"] Symbol
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>timestampNs</c>"] Trade time from the higher precision timestamp
        /// </summary>
        [JsonPropertyName("timestampNs")]
        public DateTime? TimestampPrecise { get; set; }
    }
}
