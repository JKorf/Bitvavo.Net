// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Market.
/// </summary>
public record BitvavoMarket
{
    /// <summary>
    /// Market.
    /// </summary>
    [JsonPropertyName("market")]
    public string Market { get; init; } = string.Empty;

    /// <summary>
    /// Status.
    /// </summary>
    [JsonPropertyName("status")]
    public MarketStatus Status { get; init; }

    /// <summary>
    /// Base Asset.
    /// </summary>
    [JsonPropertyName("base")]
    public string BaseAsset { get; init; } = string.Empty;

    /// <summary>
    /// Quote Asset.
    /// </summary>
    [JsonPropertyName("quote")]
    public string QuoteAsset { get; init; } = string.Empty;

    /// <summary>
    /// Price Precision.
    /// </summary>
    [JsonPropertyName("pricePrecision")]
    public string PricePrecision { get; init; } = string.Empty;

    /// <summary>
    /// Min Order In Base Asset.
    /// </summary>
    [JsonPropertyName("minOrderInBaseAsset")]
    public string MinOrderInBaseAsset { get; init; } = string.Empty;

    /// <summary>
    /// Min Order In Quote Asset.
    /// </summary>
    [JsonPropertyName("minOrderInQuoteAsset")]
    public string MinOrderInQuoteAsset { get; init; } = string.Empty;

    /// <summary>
    /// Max Order In Base Asset.
    /// </summary>
    [JsonPropertyName("maxOrderInBaseAsset")]
    public string MaxOrderInBaseAsset { get; init; } = string.Empty;

    /// <summary>
    /// Max Order In Quote Asset.
    /// </summary>
    [JsonPropertyName("maxOrderInQuoteAsset")]
    public string MaxOrderInQuoteAsset { get; init; } = string.Empty;

    /// <summary>
    /// Order Types.
    /// </summary>
    [JsonPropertyName("orderTypes")]
    public OrderType[] OrderTypes { get; init; } = System.Array.Empty<OrderType>();
}
