using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Order book snapshot
    /// </summary>
    public record BitvavoOrderBook
    {
        /// <summary>
        /// ["<c>market</c>"] Symbol
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>nonce</c>"] Order book update sequence
        /// </summary>
        [JsonPropertyName("nonce")]
        public long Nonce { get; set; }
        /// <summary>
        /// ["<c>bids</c>"] Bids in descending price order
        /// </summary>
        [JsonPropertyName("bids")]
        public BitvavoOrderBookEntry[] Bids { get; set; } = [];
        /// <summary>
        /// ["<c>asks</c>"] Asks in ascending price order
        /// </summary>
        [JsonPropertyName("asks")]
        public BitvavoOrderBookEntry[] Asks { get; set; } = [];
        /// <summary>
        /// ["<c>timestamp</c>"] Snapshot time
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime? Timestamp { get; set; }
    }

    /// <summary>
    /// Order book price level
    /// </summary>
    [JsonConverter(typeof(ArrayConverter<BitvavoOrderBookEntry>))]
    public record BitvavoOrderBookEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// Price
        /// </summary>
        [ArrayProperty(0)]
        public decimal Price { get; set; }
        /// <summary>
        /// Base asset quantity
        /// </summary>
        [ArrayProperty(1)]
        public decimal Quantity { get; set; }
    }
}
