// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;

namespace Bitvavo.Net.Objects.Models.Spot.Streams;

/// <summary>
/// Stream Fill Event.
/// </summary>
public record BitvavoStreamFillEvent
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
    /// Order ID.
    /// </summary>
    [JsonPropertyName("orderId")]
    public string OrderId { get; init; } = string.Empty;

    /// <summary>
    /// Client Order ID.
    /// </summary>
    [JsonPropertyName("clientOrderId")]
    public string? ClientOrderId { get; init; }

    /// <summary>
    /// Fill ID.
    /// </summary>
    [JsonPropertyName("fillId")]
    public string FillId { get; init; } = string.Empty;

    /// <summary>
    /// Timestamp.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; init; }

    /// <summary>
    /// Quantity.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal Quantity { get; init; }

    /// <summary>
    /// Side.
    /// </summary>
    [JsonPropertyName("side")]
    public OrderSide Side { get; init; }

    /// <summary>
    /// Price.
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; init; }

    /// <summary>
    /// Taker.
    /// </summary>
    [JsonPropertyName("taker")]
    public bool Taker { get; init; }

    /// <summary>
    /// Fee.
    /// </summary>
    [JsonPropertyName("fee")]
    public decimal Fee { get; init; }

    /// <summary>
    /// Fee Asset.
    /// </summary>
    [JsonPropertyName("feeCurrency")]
    public string FeeAsset { get; init; } = string.Empty;
}
