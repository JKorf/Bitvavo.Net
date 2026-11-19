using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Trading fees for a symbol or quote asset
    /// </summary>
    public record BitvavoSymbolFees : BitvavoAccountFees
    {
        /// <summary>
        /// ["<c>tier</c>"] Fee tier
        /// </summary>
        [JsonPropertyName("tier")]
        public int Tier { get; set; }
    }
}
