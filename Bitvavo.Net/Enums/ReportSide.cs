// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Report Side.
/// </summary>
[JsonConverter(typeof(EnumConverter<ReportSide>))]
public enum ReportSide
{
    /// <summary>
    /// ["<c>BUYI</c>"] Buy.
    /// </summary>
    [Map("BUYI")] Buy = 0,
    /// <summary>
    /// ["<c>SELL</c>"] Sell.
    /// </summary>
    [Map("SELL")] Sell = 1,
}
