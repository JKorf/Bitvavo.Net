// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Subaccount Transfer.
/// </summary>
public record BitvavoSubaccountTransfer
{
    /// <summary>
    /// Transfer ID.
    /// </summary>
    [JsonPropertyName("transferId")]
    public string TransferId { get; init; } = string.Empty;

    /// <summary>
    /// Client Request ID.
    /// </summary>
    [JsonPropertyName("clientRequestId")]
    public string? ClientRequestId { get; init; }

    /// <summary>
    /// Subaccount ID.
    /// </summary>
    [JsonPropertyName("subaccountId")]
    public string SubaccountId { get; init; } = string.Empty;

    /// <summary>
    /// Direction.
    /// </summary>
    [JsonPropertyName("direction")]
    public SubaccountTransferDirection Direction { get; init; }

    /// <summary>
    /// Symbol.
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; init; } = string.Empty;

    /// <summary>
    /// Quantity.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal? Quantity { get; init; }

    /// <summary>
    /// Status.
    /// </summary>
    [JsonPropertyName("status")]
    public TransferStatus Status { get; init; }

    /// <summary>
    /// Created At.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public System.DateTime CreatedAt { get; init; }
}

/// <summary>
/// Subaccount Transfer List.
/// </summary>
public record BitvavoSubaccountTransferList
{
    /// <summary>
    /// Items.
    /// </summary>
    [JsonPropertyName("items")]
    public BitvavoSubaccountTransfer[] Items { get; init; } = System.Array.Empty<BitvavoSubaccountTransfer>();

    /// <summary>
    /// Start.
    /// </summary>
    [JsonPropertyName("start")]
    public long Start { get; init; }

    /// <summary>
    /// End.
    /// </summary>
    [JsonPropertyName("end")]
    public long End { get; init; }

    /// <summary>
    /// Limit.
    /// </summary>
    [JsonPropertyName("limit")]
    public int Limit { get; init; }
}
