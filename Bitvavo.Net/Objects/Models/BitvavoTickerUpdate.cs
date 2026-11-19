using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Best bid, best ask, or latest trade price update
    /// </summary>
    public record BitvavoTickerUpdate
    {
        /// <summary>
        /// ["<c>market</c>"] Symbol
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>bestBid</c>"] Best bid price
        /// </summary>
        [JsonPropertyName("bestBid")]
        public decimal? BestBidPrice { get; set; }
        /// <summary>
        /// ["<c>bestBidSize</c>"] Best bid quantity
        /// </summary>
        [JsonPropertyName("bestBidSize")]
        public decimal? BestBidQuantity { get; set; }
        /// <summary>
        /// ["<c>bestAsk</c>"] Best ask price
        /// </summary>
        [JsonPropertyName("bestAsk")]
        public decimal? BestAskPrice { get; set; }
        /// <summary>
        /// ["<c>bestAskSize</c>"] Best ask quantity
        /// </summary>
        [JsonPropertyName("bestAskSize")]
        public decimal? BestAskQuantity { get; set; }
        /// <summary>
        /// ["<c>lastPrice</c>"] Latest trade price
        /// </summary>
        [JsonPropertyName("lastPrice")]
        public decimal? LastPrice { get; set; }
    }
}
