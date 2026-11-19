using Bitvavo.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Page of account transactions
    /// </summary>
    public record BitvavoTransactionHistory
    {
        /// <summary>
        /// ["<c>items</c>"] Transactions
        /// </summary>
        [JsonPropertyName("items")]
        public BitvavoTransaction[] Items { get; set; } = [];
        /// <summary>
        /// ["<c>currentPage</c>"] Current page number
        /// </summary>
        [JsonPropertyName("currentPage")]
        public int CurrentPage { get; set; }
        /// <summary>
        /// ["<c>totalPages</c>"] Total number of pages
        /// </summary>
        [JsonPropertyName("totalPages")]
        public int TotalPages { get; set; }
        /// <summary>
        /// ["<c>maxItems</c>"] Maximum transactions per page
        /// </summary>
        [JsonPropertyName("maxItems")]
        public int PageSize { get; set; }
    }

    /// <summary>
    /// Account transaction
    /// </summary>
    public record BitvavoTransaction
    {
        /// <summary>
        /// ["<c>transactionId</c>"] Transaction ID
        /// </summary>
        [JsonPropertyName("transactionId")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>executedAt</c>"] Execution time
        /// </summary>
        [JsonPropertyName("executedAt")]
        public DateTime Timestamp { get; set; }
        /// <summary>
        /// ["<c>type</c>"] Transaction type
        /// </summary>
        [JsonPropertyName("type")]
        public TransactionType Type { get; set; }
        /// <summary>
        /// ["<c>priceCurrency</c>"] Currency in which the transaction is priced
        /// </summary>
        [JsonPropertyName("priceCurrency")]
        public string? PriceAsset { get; set; }
        /// <summary>
        /// ["<c>priceAmount</c>"] Quantity in the price asset
        /// </summary>
        [JsonPropertyName("priceAmount")]
        public decimal? PriceQuantity { get; set; }
        /// <summary>
        /// ["<c>sentCurrency</c>"] Asset sent
        /// </summary>
        [JsonPropertyName("sentCurrency")]
        public string? SentAsset { get; set; }
        /// <summary>
        /// ["<c>sentAmount</c>"] Quantity sent
        /// </summary>
        [JsonPropertyName("sentAmount")]
        public decimal? SentQuantity { get; set; }
        /// <summary>
        /// ["<c>receivedCurrency</c>"] Asset received
        /// </summary>
        [JsonPropertyName("receivedCurrency")]
        public string? ReceivedAsset { get; set; }
        /// <summary>
        /// ["<c>receivedAmount</c>"] Quantity received
        /// </summary>
        [JsonPropertyName("receivedAmount")]
        public decimal? ReceivedQuantity { get; set; }
        /// <summary>
        /// ["<c>feesCurrency</c>"] Fee asset
        /// </summary>
        [JsonPropertyName("feesCurrency")]
        public string? FeeAsset { get; set; }
        /// <summary>
        /// ["<c>feesAmount</c>"] Fee quantity
        /// </summary>
        [JsonPropertyName("feesAmount")]
        public decimal? FeeQuantity { get; set; }
        /// <summary>
        /// ["<c>address</c>"] Transaction address
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }
    }
}
