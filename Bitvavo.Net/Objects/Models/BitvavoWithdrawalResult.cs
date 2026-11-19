using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Result of a withdrawal request
    /// </summary>
    public record BitvavoWithdrawalResult
    {
        /// <summary>
        /// ["<c>success</c>"] Whether the withdrawal request succeeded
        /// </summary>
        [JsonPropertyName("success")]
        public bool Success { get; set; }
        /// <summary>
        /// ["<c>symbol</c>"] Transferred asset
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>amount</c>"] Total quantity deducted from the balance
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
    }
}
