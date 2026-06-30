using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Kline (candlestick) interval as supported by Bitvavo's <c>/v2/{market}/candles</c>
/// REST endpoint and the <c>candles</c> WebSocket channel. Wire format matches Bitvavo's
/// documented strings (e.g. <c>"1m"</c>, <c>"1h"</c>, <c>"1d"</c>).
/// </summary>
[JsonConverter(typeof(EnumConverter<KlineInterval>))]
public enum KlineInterval
{
    /// <summary>
    /// 1 minute.
    /// </summary>
    [Map("1m")] 
    OneMinute = 60,
    /// <summary>
    /// 5 minutes.
    /// </summary>
    [Map("5m")] 
    FiveMinutes = 60 * 5,
    /// <summary>
    /// 15 minutes.
    /// </summary>
    [Map("15m")] 
    FifteenMinutes = 60 * 15,
    /// <summary>
    /// 30 minutes.
    /// </summary>
    [Map("30m")] 
    ThirtyMinutes = 60 * 30,
    /// <summary>
    /// 1 hour.
    /// </summary>
    [Map("1h")] 
    OneHour = 60 * 60,
    /// <summary>
    /// 2 hours.
    /// </summary>
    [Map("2h")] 
    TwoHours = 60 * 60 * 2,
    /// <summary>
    /// 4 hours.
    /// </summary>
    [Map("4h")] 
    FourHours = 60 * 60 * 4,
    /// <summary>
    /// 6 hours.
    /// </summary>
    [Map("6h")] 
    SixHours = 60 * 60 * 6,
    /// <summary>
    /// 8 hours.
    /// </summary>
    [Map("8h")] 
    EightHours = 60 * 60 * 8,
    /// <summary>
    /// 12 hours.
    /// </summary>
    [Map("12h")]
    TwelveHours = 60 * 60 * 12,
    /// <summary>
    /// 1 day.
    /// </summary>
    [Map("1d")] 
    OneDay = 60 * 60 * 24,
    /// <summary>
    /// 1 week.
    /// </summary>
    [Map("1W")] 
    OneWeek = 60 * 60 * 24 * 7,
    /// <summary>
    /// 1 month.
    /// </summary>
    [Map("1M")] 
    OneMonth = 60 * 60 * 24 * 30,
}
