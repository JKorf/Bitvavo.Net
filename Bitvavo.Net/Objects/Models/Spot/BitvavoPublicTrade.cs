// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Public Trade.
/// </summary>
public record BitvavoPublicTrade
{
    /// <summary>
    /// ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Timestamp.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; init; }

    /// <summary>
    /// Quantity.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal? Quantity { get; init; }

    /// <summary>
    /// Price.
    /// </summary>
    [JsonPropertyName("price")]
    public decimal? Price { get; init; }

    /// <summary>
    /// Side.
    /// </summary>
    [JsonPropertyName("side")]
    public OrderSide Side { get; init; }
}
