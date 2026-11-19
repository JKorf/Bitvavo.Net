using Bitvavo.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Account deposit
    /// </summary>
    public record BitvavoDeposit
    {
        /// <summary>
        /// ["<c>timestamp</c>"] Transfer timestamp
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>symbol</c>"] Transferred asset
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>amount</c>"] Transferred quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>address</c>"] Deposit address or originating bank account
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }
        /// <summary>
        /// ["<c>paymentId</c>"] Memo or tag associated with the transfer
        /// </summary>
        [JsonPropertyName("paymentId")]
        public string? PaymentId { get; set; }
        /// <summary>
        /// ["<c>txId</c>"] Blockchain transaction ID
        /// </summary>
        [JsonPropertyName("txId")]
        public string? TransactionId { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Transfer fee
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal Fee { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Deposit status when returned
        /// </summary>
        [JsonPropertyName("status")]
        public DepositStatus? Status { get; set; }
    }
}
