// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Transaction History.
/// </summary>
public record BitvavoTransactionHistory
{
    /// <summary>
    /// Items.
    /// </summary>
    [JsonPropertyName("items")]
    public BitvavoTransactionHistoryEntry[] Items { get; init; } = System.Array.Empty<BitvavoTransactionHistoryEntry>();

    /// <summary>
    /// Current Page.
    /// </summary>
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; init; }

    /// <summary>
    /// Total Pages.
    /// </summary>
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; init; }

    /// <summary>
    /// Max Items.
    /// </summary>
    [JsonPropertyName("maxItems")]
    public int MaxItems { get; init; }
}
