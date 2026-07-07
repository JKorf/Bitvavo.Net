// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Subaccount Type.
/// </summary>
[JsonConverter(typeof(EnumConverter<SubaccountType>))]
public enum SubaccountType
{
    /// <summary>
    /// ["<c>spot</c>"] Spot.
    /// </summary>
    [Map("spot")] Spot = 0,
    /// <summary>
    /// ["<c>margin</c>"] Margin.
    /// </summary>
    [Map("margin")] Margin = 1,
}
