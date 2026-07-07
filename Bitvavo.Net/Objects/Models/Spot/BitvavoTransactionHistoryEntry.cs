// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Transaction History Entry.
/// </summary>
public record BitvavoTransactionHistoryEntry
{
    /// <summary>
    /// Transaction ID.
    /// </summary>
    [JsonPropertyName("transactionId")]
    public string TransactionId { get; init; } = string.Empty;

    /// <summary>
    /// Executed At.
    /// </summary>
    [JsonPropertyName("executedAt")]
    public DateTime ExecuteTime { get; init; }

    /// <summary>
    /// Type.
    /// </summary>
    [JsonPropertyName("type")]
    public TransactionType Type { get; init; }

    /// <summary>
    /// Price Asset.
    /// </summary>
    [JsonPropertyName("priceCurrency")]
    public string? PriceAsset { get; init; }

    /// <summary>
    /// Price Quantity.
    /// </summary>
    [JsonPropertyName("priceAmount")]
    public decimal? PriceQuantity { get; init; }

    /// <summary>
    /// Sent Asset.
    /// </summary>
    [JsonPropertyName("sentCurrency")]
    public string? SentAsset { get; init; }

    /// <summary>
    /// Sent Quantity.
    /// </summary>
    [JsonPropertyName("sentAmount")]
    public decimal? SentQuantity { get; init; }

    /// <summary>
    /// Received Asset.
    /// </summary>
    [JsonPropertyName("receivedCurrency")]
    public string? ReceivedAsset { get; init; }

    /// <summary>
    /// Received Quantity.
    /// </summary>
    [JsonPropertyName("receivedAmount")]
    public decimal? ReceivedQuantity { get; init; }

    /// <summary>
    /// Fees Asset.
    /// </summary>
    [JsonPropertyName("feesCurrency")]
    public string? FeesAsset { get; init; }

    /// <summary>
    /// Fees Quantity.
    /// </summary>
    [JsonPropertyName("feesAmount")]
    public decimal? FeesQuantity { get; init; }

    /// <summary>
    /// Address.
    /// </summary>
    [JsonPropertyName("address")]
    public string? Address { get; init; }
}
