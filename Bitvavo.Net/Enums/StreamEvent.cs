// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Stream Event.
/// </summary>
[JsonConverter(typeof(EnumConverter<StreamEvent>))]
public enum StreamEvent
{
    /// <summary>
    /// ["<c>candle</c>"] Candle.
    /// </summary>
    [Map("candle")] Candle = 0,
    /// <summary>
    /// ["<c>trade</c>"] Trade.
    /// </summary>
    [Map("trade")] Trade = 1,
    /// <summary>
    /// ["<c>order</c>"] Order.
    /// </summary>
    [Map("order")] Order = 2,
    /// <summary>
    /// ["<c>fill</c>"] Fill.
    /// </summary>
    [Map("fill")] Fill = 3,
}
