// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Order Book.
/// </summary>
public record BitvavoOrderBook
{
    /// <summary>
    /// Market.
    /// </summary>
    [JsonPropertyName("market")]
    public string Market { get; init; } = string.Empty;

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
