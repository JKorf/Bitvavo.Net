// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Trades Report.
/// </summary>
public record BitvavoTradesReport
{
    /// <summary>
    /// Trade ID.
    /// </summary>
    [JsonPropertyName("tradeId")]
    public string TradeId { get; init; } = string.Empty;

    /// <summary>
    /// Transact Timestamp.
    /// </summary>
    [JsonPropertyName("transactTimestamp")]
    public DateTime TransactTime { get; init; }

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
    /// Price.
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; init; }

    /// <summary>
    /// Missing Price.
    /// </summary>
    [JsonPropertyName("missingPrice")]
    public MissingPriceStatus? MissingPrice { get; init; }

    /// <summary>
    /// Price Notation.
    /// </summary>
    [JsonPropertyName("priceNotation")]
    public string PriceNotation { get; init; } = string.Empty;

    /// <summary>
    /// Price Asset.
    /// </summary>
    [JsonPropertyName("priceCurrency")]
    public string PriceAsset { get; init; } = string.Empty;

    /// <summary>
    /// Quantity.
    /// </summary>
    [JsonPropertyName("quantity")]
    public decimal? Quantity { get; init; }

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
    /// Publication Timestamp.
    /// </summary>
    [JsonPropertyName("publicationTimestamp")]
    public DateTime PublicationTime { get; init; }

    /// <summary>
    /// Publication Venue.
    /// </summary>
    [JsonPropertyName("publicationVenue")]
    public string PublicationVenue { get; init; } = string.Empty;
}
