// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Order Side.
/// </summary>
[JsonConverter(typeof(EnumConverter<OrderSide>))]
public enum OrderSide
{
    /// <summary>
    /// ["<c>buy</c>"] Buy.
    /// </summary>
    [Map("buy")] Buy = 0,
    /// <summary>
    /// ["<c>sell</c>"] Sell.
    /// </summary>
    [Map("sell")] Sell = 1,
}
