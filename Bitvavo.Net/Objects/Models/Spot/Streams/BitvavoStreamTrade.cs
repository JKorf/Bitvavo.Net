// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Objects.Models.Spot.Streams;

/// <summary>
/// Stream Trade.
/// </summary>
public record BitvavoStreamTrade
{
    /// <summary>
    /// Event.
    /// </summary>
    [JsonPropertyName("event")]
    public StreamEvent Event { get; init; }

    /// <summary>
    /// Timestamp.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; init; }

    /// <summary>
    /// Market.
    /// </summary>
    [JsonPropertyName("market")]
    public string Market { get; init; } = string.Empty;

    /// <summary>
    /// ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Quantity.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Quantity { get; init; }

    /// <summary>
    /// Price.
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; init; }

    /// <summary>
    /// Side.
    /// </summary>
    [JsonPropertyName("side")]
    public OrderSide Side { get; init; }
}
