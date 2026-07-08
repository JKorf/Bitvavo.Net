// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;

namespace Bitvavo.Net.Objects.Models.Spot.Streams;

/// <summary>
/// Stream Order Update.
/// </summary>
public record BitvavoStreamOrderUpdate
{
    /// <summary>
    /// Event.
    /// </summary>
    [JsonPropertyName("event")]
    public StreamEvent Event { get; init; }

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
    /// Market.
    /// </summary>
    [JsonPropertyName("market")]
    public string Symbol { get; init; } = string.Empty;

    /// <summary>
    /// Created.
    /// </summary>
    [JsonPropertyName("created")]
    public DateTime Created { get; init; }

    /// <summary>
    /// Updated.
    /// </summary>
    [JsonPropertyName("updated")]
    public DateTime Updated { get; init; }

    /// <summary>
    /// Status.
    /// </summary>
    [JsonPropertyName("status")]
    public OrderStatus Status { get; init; }

    /// <summary>
    /// Side.
    /// </summary>
    [JsonPropertyName("side")]
    public OrderSide Side { get; init; }

    /// <summary>
    /// Order Type.
    /// </summary>
    [JsonPropertyName("orderType")]
    public OrderType OrderType { get; init; }

    /// <summary>
    /// Quantity.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal? Quantity { get; init; }

    /// <summary>
    /// Quantity Remaining.
    /// </summary>
    [JsonPropertyName("amountRemaining")]
    public decimal? QuantityRemaining { get; init; }

    /// <summary>
    /// Price.
    /// </summary>
    [JsonPropertyName("price")]
    public decimal? Price { get; init; }

    /// <summary>
    /// On Hold.
    /// </summary>
    [JsonPropertyName("onHold")]
    public decimal? OnHold { get; init; }

    /// <summary>
    /// On Hold Asset.
    /// </summary>
    [JsonPropertyName("onHoldCurrency")]
    public string? OnHoldAsset { get; init; }

    /// <summary>
    /// Trigger Quantity.
    /// </summary>
    [JsonPropertyName("triggerAmount")]
    public decimal? TriggerQuantity { get; init; }

    /// <summary>
    /// Trigger Price.
    /// </summary>
    [JsonPropertyName("triggerPrice")]
    public decimal? TriggerPrice { get; init; }

    /// <summary>
    /// Trigger Type.
    /// </summary>
    [JsonPropertyName("triggerType")]
    public TriggerType? TriggerType { get; init; }

    /// <summary>
    /// Trigger Reference.
    /// </summary>
    [JsonPropertyName("triggerReference")]
    public TriggerReference? TriggerReference { get; init; }

    /// <summary>
    /// Time In Force.
    /// </summary>
    [JsonPropertyName("timeInForce")]
    public TimeInForce? TimeInForce { get; init; }

    /// <summary>
    /// Post Only.
    /// </summary>
    [JsonPropertyName("postOnly")]
    public bool? PostOnly { get; init; }

    /// <summary>
    /// Self Trade Prevention.
    /// </summary>
    [JsonPropertyName("selfTradePrevention")]
    public SelfTradePrevention? SelfTradePrevention { get; init; }

    /// <summary>
    /// Visible.
    /// </summary>
    [JsonPropertyName("visible")]
    public bool? Visible { get; init; }

    /// <summary>
    /// Filled Quantity.
    /// </summary>
    [JsonPropertyName("filledAmount")]
    public decimal? FilledQuantity { get; init; }

    /// <summary>
    /// Filled Quote Quantity.
    /// </summary>
    [JsonPropertyName("filledAmountQuote")]
    public decimal? FilledQuoteQuantity { get; init; }

    /// <summary>
    /// Execution Type.
    /// </summary>
    [JsonPropertyName("executionType")]
    public string? ExecutionType { get; init; }

    /// <summary>
    /// Restatement Reason.
    /// </summary>
    [JsonPropertyName("restatementReason")]
    public string? RestatementReason { get; init; }

    /// <summary>
    /// COD Group ID.
    /// </summary>
    [JsonPropertyName("codGroupId")]
    public int? CodGroupId { get; init; }
}
