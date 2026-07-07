// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System;
using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Kline.
/// </summary>
[JsonConverter(typeof(ArrayConverter<BitvavoKline>))]
public record BitvavoKline
{
    /// <summary>
    /// Open Time.
    /// </summary>
    [ArrayProperty(0)]
    public DateTime OpenTime { get; set; }

    /// <summary>
    /// Open Price.
    /// </summary>
    [ArrayProperty(1)]
    public decimal OpenPrice { get; set; }

    /// <summary>
    /// High Price.
    /// </summary>
    [ArrayProperty(2)]
    public decimal HighPrice { get; set; }

    /// <summary>
    /// Low Price.
    /// </summary>
    [ArrayProperty(3)]
    public decimal LowPrice { get; set; }

    /// <summary>
    /// Close Price.
    /// </summary>
    [ArrayProperty(4)]
    public decimal ClosePrice { get; set; }

    /// <summary>
    /// Volume.
    /// </summary>
    [ArrayProperty(5)]
    public decimal Volume { get; set; }
}
