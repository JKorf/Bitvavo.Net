// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using Bitvavo.Net.Enums;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Order Book.
/// </summary>
public record BitvavoOrderBookUpdate
{
    /// <summary>
    /// Event.
    /// </summary>
    [JsonPropertyName("event")]
    public StreamEvent Event { get; init; }
    /// <summary>
    /// Market.
    /// </summary>
    [JsonPropertyName("market")]
    public string Symbol { get; init; } = string.Empty;

    /// <summary>
    /// Nonce.
    /// </summary>
    [JsonPropertyName("nonce")]
    public long Nonce { get; init; }

    /// <summary>
    /// Bids.
    /// </summary>
    [JsonPropertyName("bids")]
    public BitvavoOrderBookEntry[] Bids { get; init; } = System.Array.Empty<BitvavoOrderBookEntry>();

    /// <summary>
    /// Asks.
    /// </summary>
    [JsonPropertyName("asks")]
    public BitvavoOrderBookEntry[] Asks { get; init; } = System.Array.Empty<BitvavoOrderBookEntry>();
}
