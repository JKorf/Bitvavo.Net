using Bitvavo.Net.Enums;
using CryptoExchange.Net.Interfaces;
using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// MiCA order book report
    /// </summary>
    public record BitvavoOrderBookReport
    {
        /// <summary>
        /// ["<c>submissionTimestamp</c>"] Submission time when available
        /// </summary>
        [JsonPropertyName("submissionTimestamp")]
        public DateTime? SubmissionTimestamp { get; set; }
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
        /// ["<c>bids</c>"] Bid levels
        /// </summary>
        [JsonPropertyName("bids")]
        public BitvavoOrderBookReportEntry[] Bids { get; set; } = [];
        /// <summary>
        /// ["<c>asks</c>"] Ask levels
        /// </summary>
        [JsonPropertyName("asks")]
        public BitvavoOrderBookReportEntry[] Asks { get; set; } = [];
        /// <summary>
        /// ["<c>tradingSystem</c>"] Trading system identifier
        /// </summary>
        [JsonPropertyName("tradingSystem")]
        public string TradingSystem { get; set; } = string.Empty;
    }

    /// <summary>
    /// MiCA order book price level
    /// </summary>
    public record BitvavoOrderBookReportEntry : ISymbolOrderBookEntry
    {
        /// <summary>
        /// ["<c>side</c>"] Order side
        /// </summary>
        [JsonPropertyName("side")]
        public ReportOrderSide Side { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
        /// <summary>
        /// ["<c>size</c>"] Base asset quantity
        /// </summary>
        [JsonPropertyName("size")]
        public decimal Quantity { get; set; }
        /// <summary>
        /// ["<c>numOrders</c>"] Number of orders at this price level
        /// </summary>
        [JsonPropertyName("numOrders")]
        public int OrderCount { get; set; }
        // The order book report schema also lists quantity as the wire name.
        [JsonInclude]
        [JsonPropertyName("quantity")]
        internal decimal ReportedQuantity
        {
            get => Quantity;
            set => Quantity = value;
        }
    }
}
