// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Cancel Orders After.
/// </summary>
public sealed record BitvavoCancelOrdersAfter
{
    /// <summary>
    /// COD Group ID.
    /// </summary>
    [JsonPropertyName("codGroupId")]
    public int CodGroupId { get; init; }

    /// <summary>
    /// Time Of Expiry Seconds.
    /// </summary>
    [JsonPropertyName("timeOfExpirySeconds")]
    public long TimeOfExpirySeconds { get; init; }
}
