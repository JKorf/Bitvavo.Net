// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Balance.
/// </summary>
public record BitvavoBalance
{
    /// <summary>
    /// Symbol.
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; init; } = string.Empty;

    /// <summary>
    /// Available.
    /// </summary>
    [JsonPropertyName("available")]
    public decimal? Available { get; init; }

    /// <summary>
    /// In Order.
    /// </summary>
    [JsonPropertyName("inOrder")]
    public decimal? InOrder { get; init; }
}
