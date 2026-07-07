// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Market Fee.
/// </summary>
public record BitvavoMarketFee
{
    /// <summary>
    /// Tier.
    /// </summary>
    [JsonPropertyName("tier")]
    public string Tier { get; init; } = string.Empty;

    /// <summary>
    /// Volume.
    /// </summary>
    [JsonPropertyName("volume")]
    public decimal? Volume { get; init; }

    /// <summary>
    /// Taker.
    /// </summary>
    [JsonPropertyName("taker")]
    public decimal? Taker { get; init; }

    /// <summary>
    /// Maker.
    /// </summary>
    [JsonPropertyName("maker")]
    public decimal? Maker { get; init; }
}
