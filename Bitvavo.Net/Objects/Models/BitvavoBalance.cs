using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Account balance for an asset
    /// </summary>
    public record BitvavoBalance
    {
        /// <summary>
        /// ["<c>symbol</c>"] Asset
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>available</c>"] Quantity available to use
        /// </summary>
        [JsonPropertyName("available")]
        public decimal AvailableQuantity { get; set; }
        /// <summary>
        /// ["<c>inOrder</c>"] Quantity reserved for open orders
        /// </summary>
        [JsonPropertyName("inOrder")]
        public decimal InOrderQuantity { get; set; }
    }
}
