using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Latest trade price for a symbol
    /// </summary>
    public record BitvavoTickerPrice
    {
        /// <summary>
        /// ["<c>market</c>"] Symbol
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>price</c>"] Latest trade price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
    }
}
