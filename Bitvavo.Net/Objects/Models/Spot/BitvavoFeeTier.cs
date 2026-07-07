// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Fee Tier.
/// </summary>
public record BitvavoFeeTier
{
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

    /// <summary>
    /// Volume.
    /// </summary>
    [JsonPropertyName("volume")]
    public decimal? Volume { get; init; }
}
