// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Account Info.
/// </summary>
public record BitvavoAccountInfo
{
    /// <summary>
    /// Fees.
    /// </summary>
    [JsonPropertyName("fees")]
    public BitvavoFeeTier Fees { get; init; } = new();

    /// <summary>
    /// Capabilities.
    /// </summary>
    [JsonPropertyName("capabilities")]
    public AccountCapability[] Capabilities { get; init; } = System.Array.Empty<AccountCapability>();
}
