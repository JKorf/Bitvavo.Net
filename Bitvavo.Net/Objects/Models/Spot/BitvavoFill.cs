// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Fill.
/// </summary>
public record BitvavoFill
{
    /// <summary>
    /// ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Order ID.
    /// </summary>
    [JsonPropertyName("orderId")]
    public string? OrderId { get; init; }

    /// <summary>
    /// Client Order ID.
    /// </summary>
    [JsonPropertyName("clientOrderId")]
    public string? ClientOrderId { get; init; }

    /// <summary>
    /// Timestamp.
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; init; }

    /// <summary>
    /// Market.
    /// </summary>
    [JsonPropertyName("market")]
    public string? Market { get; init; }

    /// <summary>
    /// Side.
    /// </summary>
    [JsonPropertyName("side")]
    public OrderSide? Side { get; init; }

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
    /// Taker.
    /// </summary>
    [JsonPropertyName("taker")]
    public bool? Taker { get; init; }

    /// <summary>
    /// Fee.
    /// </summary>
    [JsonPropertyName("fee")]
    public decimal? Fee { get; init; }

    /// <summary>
    /// Fee Asset.
    /// </summary>
    [JsonPropertyName("feeCurrency")]
    public string? FeeAsset { get; init; }

    /// <summary>
    /// Settled.
    /// </summary>
    [JsonPropertyName("settled")]
    public bool? Settled { get; init; }
}
