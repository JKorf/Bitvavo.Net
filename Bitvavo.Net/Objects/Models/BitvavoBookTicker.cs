using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Best bid and ask for a symbol
    /// </summary>
    public record BitvavoBookTicker
    {
        /// <summary>
        /// ["<c>market</c>"] Symbol
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>bid</c>"] Best bid price
        /// </summary>
        [JsonPropertyName("bid")]
        public decimal? BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>bidSize</c>"] Best bid quantity
        /// </summary>
        [JsonPropertyName("bidSize")]
        public decimal? BestBidQuantity { get; set; }
        /// <summary>
        /// ["<c>ask</c>"] Best ask price
        /// </summary>
        [JsonPropertyName("ask")]
        public decimal? BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>askSize</c>"] Best ask quantity
        /// </summary>
        [JsonPropertyName("askSize")]
        public decimal? BestAskQuantity { get; set; }
    }
}
