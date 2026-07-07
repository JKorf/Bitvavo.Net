// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Kline Interval.
/// </summary>
[JsonConverter(typeof(EnumConverter<KlineInterval>))]
public enum KlineInterval
{
#warning Value to seconds
    /// <summary>
    /// ["<c>1m</c>"] One Minute.
    /// </summary>
    [Map("1m")] OneMinute = 0,
    /// <summary>
    /// ["<c>5m</c>"] Five Minutes.
    /// </summary>
    [Map("5m")] FiveMinutes = 1,
    /// <summary>
    /// ["<c>15m</c>"] Fifteen Minutes.
    /// </summary>
    [Map("15m")] FifteenMinutes = 2,
    /// <summary>
    /// ["<c>30m</c>"] Thirty Minutes.
    /// </summary>
    [Map("30m")] ThirtyMinutes = 3,
    /// <summary>
    /// ["<c>1h</c>"] One Hour.
    /// </summary>
    [Map("1h")] OneHour = 4,
    /// <summary>
    /// ["<c>2h</c>"] Two Hours.
    /// </summary>
    [Map("2h")] TwoHours = 5,
    /// <summary>
    /// ["<c>4h</c>"] Four Hours.
    /// </summary>
    [Map("4h")] FourHours = 6,
    /// <summary>
    /// ["<c>6h</c>"] Six Hours.
    /// </summary>
    [Map("6h")] SixHours = 7,
    /// <summary>
    /// ["<c>8h</c>"] Eight Hours.
    /// </summary>
    [Map("8h")] EightHours = 8,
    /// <summary>
    /// ["<c>12h</c>"] Twelve Hours.
    /// </summary>
    [Map("12h")] TwelveHours = 9,
    /// <summary>
    /// ["<c>1d</c>"] One Day.
    /// </summary>
    [Map("1d")] OneDay = 10,
    /// <summary>
    /// ["<c>1W</c>"] One Week.
    /// </summary>
    [Map("1W")] OneWeek = 11,
    /// <summary>
    /// ["<c>1M</c>"] One Month.
    /// </summary>
    [Map("1M")] OneMonth = 12,
}
