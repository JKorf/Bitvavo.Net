// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Time In Force.
/// </summary>
[JsonConverter(typeof(EnumConverter<TimeInForce>))]
public enum TimeInForce
{
    /// <summary>
    /// ["<c>GTC</c>"] Good Till Canceled.
    /// </summary>
    [Map("GTC")] GoodTillCanceled = 0,
    /// <summary>
    /// ["<c>IOC</c>"] Immediate Or Cancel.
    /// </summary>
    [Map("IOC")] ImmediateOrCancel = 1,
    /// <summary>
    /// ["<c>FOK</c>"] Fill Or Kill.
    /// </summary>
    [Map("FOK")] FillOrKill = 2,
}
