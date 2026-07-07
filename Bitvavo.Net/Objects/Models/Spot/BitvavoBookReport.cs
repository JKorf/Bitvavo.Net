// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Book Report.
/// </summary>
public record BitvavoBookReport
{
    /// <summary>
    /// Submission Timestamp.
    /// </summary>
    [JsonPropertyName("submissionTimestamp")]
    public DateTime SubmissionTimestamp { get; init; }

    /// <summary>
    /// Asset Code.
    /// </summary>
    [JsonPropertyName("assetCode")]
    public string AssetCode { get; init; } = string.Empty;

    /// <summary>
    /// Asset Name.
    /// </summary>
    [JsonPropertyName("assetName")]
    public string AssetName { get; init; } = string.Empty;

    /// <summary>
    /// Price Asset.
    /// </summary>
    [JsonPropertyName("priceCurrency")]
    public string PriceAsset { get; init; } = string.Empty;

    /// <summary>
    /// Price Notation.
    /// </summary>
    [JsonPropertyName("priceNotation")]
    public string PriceNotation { get; init; } = string.Empty;

    /// <summary>
    /// Quantity Asset.
    /// </summary>
    [JsonPropertyName("quantityCurrency")]
    public string QuantityAsset { get; init; } = string.Empty;

    /// <summary>
    /// Quantity Notation.
    /// </summary>
    [JsonPropertyName("quantityNotation")]
    public string QuantityNotation { get; init; } = string.Empty;

    /// <summary>
    /// Venue.
    /// </summary>
    [JsonPropertyName("venue")]
    public string Venue { get; init; } = string.Empty;

    /// <summary>
    /// Trading System.
    /// </summary>
    [JsonPropertyName("tradingSystem")]
    public string TradingSystem { get; init; } = string.Empty;

    /// <summary>
    /// Publication Timestamp.
    /// </summary>
    [JsonPropertyName("publicationTimestamp")]
    public DateTime PublicationTimestamp { get; init; }

    /// <summary>
    /// Bids.
    /// </summary>
    [JsonPropertyName("bids")]
    public BitvavoBookReportEntry[] Bids { get; init; } = System.Array.Empty<BitvavoBookReportEntry>();

    /// <summary>
    /// Asks.
    /// </summary>
    [JsonPropertyName("asks")]
    public BitvavoBookReportEntry[] Asks { get; init; } = System.Array.Empty<BitvavoBookReportEntry>();
}

/// <summary>
/// Book Report Entry.
/// </summary>
public record BitvavoBookReportEntry
{
    /// <summary>
    /// Side.
    /// </summary>
    [JsonPropertyName("side")]
    public ReportSide Side { get; init; }

    /// <summary>
    /// Price.
    /// </summary>
    [JsonPropertyName("price")]
    public decimal? Price { get; init; }

    /// <summary>
    /// Quantity.
    /// </summary>
    [JsonPropertyName("quantity")]
    public decimal? Quantity { get; init; }

    /// <summary>
    /// Num Orders.
    /// </summary>
    [JsonPropertyName("numOrders")]
    public int NumOrders { get; init; }
}
