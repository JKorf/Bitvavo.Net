using Bitvavo.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// MiCA trade report
    /// </summary>
    public record BitvavoTradeReport
    {
        /// <summary>
        /// ["<c>tradeId</c>"] Trade ID
        /// </summary>
        [JsonPropertyName("tradeId")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>transactTimestamp</c>"] Transaction time
        /// </summary>
        [JsonPropertyName("transactTimestamp")]
        public DateTime TransactionTimestamp { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Trade price when available
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// ["<c>missingPrice</c>"] Reason the trade price may be unavailable
        /// </summary>
        [JsonPropertyName("missingPrice")]
        public ReportMissingPrice? MissingPrice { get; set; }
        /// <summary>
        /// ["<c>quantity</c>"] Base asset quantity
        /// </summary>
        [JsonPropertyName("quantity")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>assetCode</c>"] Digital Token Identifier or asset symbol
        /// </summary>
        [JsonPropertyName("assetCode")]
        public string AssetCode { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>assetName</c>"] Asset name
        /// </summary>
        [JsonPropertyName("assetName")]
        public string AssetName { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>priceCurrency</c>"] Currency in which prices are expressed
        /// </summary>
        [JsonPropertyName("priceCurrency")]
        public string PriceCurrency { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>priceNotation</c>"] Price notation
        /// </summary>
        [JsonPropertyName("priceNotation")]
        public ReportPriceNotation PriceNotation { get; set; }
        /// <summary>
        /// ["<c>quantityCurrency</c>"] Currency in which quantities are expressed
        /// </summary>
        [JsonPropertyName("quantityCurrency")]
        public string QuantityCurrency { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>quantityNotation</c>"] Quantity notation
        /// </summary>
        [JsonPropertyName("quantityNotation")]
        public ReportQuantityNotation QuantityNotation { get; set; }
        /// <summary>
        /// ["<c>venue</c>"] Trading venue identifier
        /// </summary>
        [JsonPropertyName("venue")]
        public string Venue { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>publicationTimestamp</c>"] Publication time
        /// </summary>
        [JsonPropertyName("publicationTimestamp")]
        public DateTime PublicationTimestamp { get; set; }
        /// <summary>
        /// ["<c>publicationVenue</c>"] Publication venue identifier
        /// </summary>
        [JsonPropertyName("publicationVenue")]
        public string PublicationVenue { get; set; } = string.Empty;
    }
}
