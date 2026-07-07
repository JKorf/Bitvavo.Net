// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Ticker24h.
/// </summary>
public record BitvavoTicker24h
{
    /// <summary>
    /// Symbol.
    /// </summary>
    [JsonPropertyName("market")]
    public string Symbol { get; init; } = string.Empty;

    /// <summary>
    /// Start Timestamp.
    /// </summary>
    [JsonPropertyName("startTimestamp")]
    public DateTime? StartTime { get; init; }

    /// <summary>
    /// Timestamp.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime? Timestamp { get; init; }

    /// <summary>
    /// Open.
    /// </summary>
    [JsonPropertyName("open")]
    public decimal? OpenPrice { get; init; }

    /// <summary>
    /// Open Timestamp.
    /// </summary>
    [JsonPropertyName("openTimestamp")]
    public DateTime? OpenTime { get; init; }

    /// <summary>
    /// High.
    /// </summary>
    [JsonPropertyName("high")]
    public decimal? HighPrice { get; init; }

    /// <summary>
    /// Low.
    /// </summary>
    [JsonPropertyName("low")]
    public decimal? LowPrice { get; init; }

    /// <summary>
    /// Last.
    /// </summary>
    [JsonPropertyName("last")]
    public decimal? LastPrice { get; init; }

    /// <summary>
    /// Close Timestamp.
    /// </summary>
    [JsonPropertyName("closeTimestamp")]
    public DateTime? CloseTime { get; init; }

    /// <summary>
    /// Bid.
    /// </summary>
    [JsonPropertyName("bid")]
    public decimal? BestBidPrice { get; init; }

    /// <summary>
    /// Bid Quantity.
    /// </summary>
    [JsonPropertyName("bidSize")]
    public decimal? BestBidQuantity { get; init; }

    /// <summary>
    /// Ask.
    /// </summary>
    [JsonPropertyName("ask")]
    public decimal? BestAskPrice { get; init; }

    /// <summary>
    /// Ask Quantity.
    /// </summary>
    [JsonPropertyName("askSize")]
    public decimal? BestAskQuantity { get; init; }

    /// <summary>
    /// Volume.
    /// </summary>
    [JsonPropertyName("volume")]
    public decimal? Volume { get; init; }

    /// <summary>
    /// Volume Quote.
    /// </summary>
    [JsonPropertyName("volumeQuote")]
    public decimal? QuoteVolume { get; init; }
}
