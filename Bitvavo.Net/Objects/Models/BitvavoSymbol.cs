using Bitvavo.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Trading symbol information
    /// </summary>
    public record BitvavoSymbol
    {
        /// <summary>
        /// ["<c>market</c>"] Trading symbol
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>status</c>"] Trading status
        /// </summary>
        [JsonPropertyName("status")]
        public SymbolStatus Status { get; set; }
        /// <summary>
        /// ["<c>base</c>"] Base asset
        /// </summary>
        [JsonPropertyName("base")]
        public string BaseAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>quote</c>"] Quote asset
        /// </summary>
        [JsonPropertyName("quote")]
        public string QuoteAsset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>pricePrecision</c>"] Legacy price precision
        /// </summary>
        [Obsolete("Use TickSize for the price increment.")]
        [JsonPropertyName("pricePrecision")]
        public int? PricePrecision { get; set; }
        /// <summary>
        /// ["<c>minOrderInBaseAsset</c>"] Minimum order quantity in the base asset
        /// </summary>
        [JsonPropertyName("minOrderInBaseAsset")]
        public decimal MinOrderQuantity { get; set; }
        /// <summary>
        /// ["<c>minOrderInQuoteAsset</c>"] Minimum order quantity in the quote asset
        /// </summary>
        [JsonPropertyName("minOrderInQuoteAsset")]
        public decimal MinOrderQuoteQuantity { get; set; }
        /// <summary>
        /// ["<c>maxOrderInBaseAsset</c>"] Maximum order quantity in the base asset
        /// </summary>
        [JsonPropertyName("maxOrderInBaseAsset")]
        public decimal MaxOrderQuantity { get; set; }
        /// <summary>
        /// ["<c>maxOrderInQuoteAsset</c>"] Maximum order quantity in the quote asset
        /// </summary>
        [JsonPropertyName("maxOrderInQuoteAsset")]
        public decimal MaxOrderQuoteQuantity { get; set; }
        /// <summary>
        /// ["<c>quantityDecimals</c>"] Maximum number of decimal places in the base asset quantity
        /// </summary>
        [JsonPropertyName("quantityDecimals")]
        public int QuantityDecimals { get; set; }
        /// <summary>
        /// ["<c>notionalDecimals</c>"] Maximum number of decimal places in the quote asset quantity
        /// </summary>
        [JsonPropertyName("notionalDecimals")]
        public int QuoteQuantityDecimals { get; set; }
        /// <summary>
        /// ["<c>tickSize</c>"] Minimum price increment
        /// </summary>
        [JsonPropertyName("tickSize")]
        public decimal TickSize { get; set; }
        /// <summary>
        /// ["<c>maxOpenOrders</c>"] Maximum open orders per account for this symbol
        /// </summary>
        [JsonPropertyName("maxOpenOrders")]
        public int MaxOpenOrders { get; set; }
        /// <summary>
        /// ["<c>feeCategory</c>"] Fee category code
        /// </summary>
        [JsonPropertyName("feeCategory")]
        public string FeeCategory { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>orderTypes</c>"] Supported order types
        /// </summary>
        [JsonPropertyName("orderTypes")]
        public OrderType[] OrderTypes { get; set; } = [];
    }
}
