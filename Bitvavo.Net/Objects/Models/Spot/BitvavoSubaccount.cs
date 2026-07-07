// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Subaccount.
/// </summary>
public record BitvavoSubaccount
{
    /// <summary>
    /// ID.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; init; } = string.Empty;

    /// <summary>
    /// Type.
    /// </summary>
    [JsonPropertyName("type")]
    public SubaccountType Type { get; init; }

    /// <summary>
    /// Status.
    /// </summary>
    [JsonPropertyName("status")]
    public SubaccountStatus Status { get; init; }

    /// <summary>
    /// Label.
    /// </summary>
    [JsonPropertyName("label")]
    public string? Label { get; init; }
}

/// <summary>
/// Subaccount List.
/// </summary>
public record BitvavoSubaccountList
{
    /// <summary>
    /// Items.
    /// </summary>
    [JsonPropertyName("items")]
    public BitvavoSubaccount[] Items { get; init; } = System.Array.Empty<BitvavoSubaccount>();

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
