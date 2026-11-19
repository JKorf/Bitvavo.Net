using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Expiry for a cancel-on-disconnect group
    /// </summary>
    public record BitvavoCancelOrdersAfter
    {
        /// <summary>
        /// ["<c>codGroupId</c>"] Cancel-on-disconnect group ID
        /// </summary>
        [JsonPropertyName("codGroupId")]
        public int CancelGroupId { get; set; }
        /// <summary>
        /// ["<c>timeOfExpirySeconds</c>"] Time when Bitvavo will cancel the group
        /// </summary>
        [JsonPropertyName("timeOfExpirySeconds")]
        public DateTime ExpiryTime { get; set; }
    }
}
