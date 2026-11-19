using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// 24-hour ticker data
    /// </summary>
    public record BitvavoTicker : BitvavoBookTicker
    {
        /// <summary>
        /// ["<c>startTimestamp</c>"] Start of the 24-hour period
        /// </summary>
        [JsonPropertyName("startTimestamp")]
        public DateTime? StartTime { get; set; }
        /// <summary>
        /// ["<c>timestamp</c>"] Time of the ticker snapshot
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }
        /// <summary>
        /// ["<c>open</c>"] Opening price
        /// </summary>
        [JsonPropertyName("open")]
        public decimal? OpenPrice { get; set; }
        /// <summary>
        /// ["<c>openTimestamp</c>"] Time of the opening trade
        /// </summary>
        [JsonPropertyName("openTimestamp")]
        public DateTime? OpenTime { get; set; }
        /// <summary>
        /// ["<c>high</c>"] Highest trade price
        /// </summary>
        [JsonPropertyName("high")]
        public decimal? HighPrice { get; set; }
        /// <summary>
        /// ["<c>low</c>"] Lowest trade price
        /// </summary>
        [JsonPropertyName("low")]
        public decimal? LowPrice { get; set; }
        /// <summary>
        /// ["<c>last</c>"] Latest trade price
        /// </summary>
        [JsonPropertyName("last")]
        public decimal? LastPrice { get; set; }
        /// <summary>
        /// ["<c>closeTimestamp</c>"] Time of the closing trade
        /// </summary>
        [JsonPropertyName("closeTimestamp")]
        public DateTime? CloseTime { get; set; }
        /// <summary>
        /// ["<c>volume</c>"] Traded base asset quantity
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal? BaseQuantity { get; set; }
        /// <summary>
        /// ["<c>volumeQuote</c>"] Traded quote asset quantity
        /// </summary>
        [JsonPropertyName("volumeQuote")]
        public decimal? QuoteQuantity { get; set; }
    }
}
