// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Order ID.
/// </summary>
public record BitvavoOrderId
{
    /// <summary>
    /// Order ID.
    /// </summary>
    [JsonPropertyName("orderId")]
    public string OrderId { get; init; } = string.Empty;

    /// <summary>
    /// Market.
    /// </summary>
    [JsonPropertyName("market")]
    public string? Market { get; init; }
}
