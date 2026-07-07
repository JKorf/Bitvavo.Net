// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Withdrawal Result.
/// </summary>
public record BitvavoWithdrawalResult
{
    /// <summary>
    /// Success.
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; init; }

    /// <summary>
    /// Asset.
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Asset { get; init; } = string.Empty;

    /// <summary>
    /// Quantity.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal? Quantity { get; init; }
}
