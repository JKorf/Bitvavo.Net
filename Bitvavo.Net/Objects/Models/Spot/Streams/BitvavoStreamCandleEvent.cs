// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;

namespace Bitvavo.Net.Objects.Models.Spot.Streams;

/// <summary>
/// Stream Candle Event.
/// </summary>
public record BitvavoStreamCandleEvent
{
    /// <summary>
    /// Event.
    /// </summary>
    [JsonPropertyName("event")]
    public StreamEvent Event { get; init; }

    /// <summary>
    /// Market.
    /// </summary>
    [JsonPropertyName("market")]
    public string Symbol { get; init; } = string.Empty;

    /// <summary>
    /// Interval.
    /// </summary>
    [JsonPropertyName("interval")]
    public KlineInterval Interval { get; init; }

    /// <summary>
    /// Candle.
    /// </summary>
    [JsonPropertyName("candle")]
    public BitvavoKline[] Candle { get; init; } = System.Array.Empty<BitvavoKline>();
}
