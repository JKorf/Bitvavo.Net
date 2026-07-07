// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Ticker Price.
/// </summary>
public record BitvavoTickerPrice
{
    /// <summary>
    /// Symbol.
    /// </summary>
    [JsonPropertyName("market")]
    public string Symbol { get; init; } = string.Empty;

    /// <summary>
    /// Price.
    /// </summary>
    [JsonPropertyName("price")]
    public decimal? Price { get; init; }
}
