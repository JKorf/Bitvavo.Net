// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Trigger Reference.
/// </summary>
[JsonConverter(typeof(EnumConverter<TriggerReference>))]
public enum TriggerReference
{
    /// <summary>
    /// ["<c>lastTrade</c>"] Last Trade.
    /// </summary>
    [Map("lastTrade")] LastTrade = 0,
    /// <summary>
    /// ["<c>bestBid</c>"] Best Bid.
    /// </summary>
    [Map("bestBid")] BestBid = 1,
    /// <summary>
    /// ["<c>bestAsk</c>"] Best Ask.
    /// </summary>
    [Map("bestAsk")] BestAsk = 2,
    /// <summary>
    /// ["<c>midPrice</c>"] Mid Price.
    /// </summary>
    [Map("midPrice")] MidPrice = 3,
}
