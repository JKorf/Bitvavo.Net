// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Server Time.
/// </summary>
public record BitvavoServerTime
{
    /// <summary>
    /// Time.
    /// </summary>
    [JsonPropertyName("time")]
    public DateTime Time { get; init; }

    /// <summary>
    /// Time Ns.
    /// </summary>
    [JsonPropertyName("timeNs")]
    public long TimeNs { get; init; }
}
