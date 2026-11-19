using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Order fill
    /// </summary>
    public record BitvavoOrderFill
    {
        /// <summary>
        /// ["<c>id</c>"] Fill ID
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>timestamp</c>"] Execution time
        /// </summary>
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>amount</c>"] Traded base asset quantity
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Execution price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>taker</c>"] Whether the account took liquidity
        /// </summary>
        [JsonPropertyName("taker")]
        public bool IsTaker { get; set; }
        /// <summary>
        /// ["<c>fee</c>"] Fee paid or negative rebate
        /// </summary>
        [JsonPropertyName("fee")]
        public decimal? FeeQuantity { get; set; }
        /// <summary>
        /// ["<c>feeCurrency</c>"] Asset in which the fee is paid
        /// </summary>
        [JsonPropertyName("feeCurrency")]
        public string? FeeAsset { get; set; }
        /// <summary>
        /// ["<c>settled</c>"] Whether the fee and exchanged assets have settled
        /// </summary>
        [JsonPropertyName("settled")]
        public bool Settled { get; set; }
    }
}
