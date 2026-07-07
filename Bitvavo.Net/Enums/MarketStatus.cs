// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Market Status.
/// </summary>
[JsonConverter(typeof(EnumConverter<MarketStatus>))]
public enum MarketStatus
{
    /// <summary>
    /// ["<c>trading</c>"] Trading.
    /// </summary>
    [Map("trading")] Trading = 0,
    /// <summary>
    /// ["<c>halted</c>"] Halted.
    /// </summary>
    [Map("halted")] Halted = 1,
    /// <summary>
    /// ["<c>auction</c>"] Auction.
    /// </summary>
    [Map("auction")] Auction = 2,
}
