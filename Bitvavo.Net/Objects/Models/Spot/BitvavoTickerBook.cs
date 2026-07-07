// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Ticker Book.
/// </summary>
public record BitvavoTickerBook
{
    /// <summary>
    /// Symbol.
    /// </summary>
    [JsonPropertyName("market")]
    public string Symbol { get; init; } = string.Empty;

    /// <summary>
    /// Best bid price.
    /// </summary>
    [JsonPropertyName("bid")]
    public decimal? BestBidPrice { get; init; }

    /// <summary>
    /// Best bid quantity.
    /// </summary>
    [JsonPropertyName("bidSize")]
    public decimal? BestBidQuantity { get; init; }

    /// <summary>
    /// Best ask price
    /// </summary>
    [JsonPropertyName("ask")]
    public decimal? BestAskPrice { get; init; }

    /// <summary>
    /// Best Ask Quantity.
    /// </summary>
    [JsonPropertyName("askSize")]
    public decimal? BestAskQuantity { get; init; }
}
