using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Snapshot of the order book for a single market
/// </summary>
public record BitvavoOrderBook
{
    /// <summary>
    /// Market identifier.
    /// </summary>
    [JsonPropertyName("market")]
    public string Market { get; init; } = string.Empty;

    /// <summary>
    /// Monotonically-increasing identifier â€” every order-book change increments it. Use it
    /// to detect missed updates when reconciling against the WebSocket book channel.
    /// </summary>
    [JsonPropertyName("nonce")]
    public long Nonce { get; init; }

    /// <summary>
    /// Buy orders, sorted by descending price (best bid first).
    /// </summary>
    [JsonPropertyName("bids")]
    public BitvavoOrderBookEntry[] Bids { get; init; } = [];

    /// <summary>
    /// Sell orders, sorted by ascending price (best ask first).
    /// </summary>
    [JsonPropertyName("asks")]
    public BitvavoOrderBookEntry[] Asks { get; init; } = [];
}
