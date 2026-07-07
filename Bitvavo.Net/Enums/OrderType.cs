// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Order Type.
/// </summary>
[JsonConverter(typeof(EnumConverter<OrderType>))]
public enum OrderType
{
    /// <summary>
    /// ["<c>market</c>"] Market.
    /// </summary>
    [Map("market")] Market = 0,
    /// <summary>
    /// ["<c>limit</c>"] Limit.
    /// </summary>
    [Map("limit")] Limit = 1,
    /// <summary>
    /// ["<c>stopLoss</c>"] Stop Loss.
    /// </summary>
    [Map("stopLoss")] StopLoss = 2,
    /// <summary>
    /// ["<c>stopLossLimit</c>"] Stop Loss Limit.
    /// </summary>
    [Map("stopLossLimit")] StopLossLimit = 3,
    /// <summary>
    /// ["<c>takeProfit</c>"] Take Profit.
    /// </summary>
    [Map("takeProfit")] TakeProfit = 4,
    /// <summary>
    /// ["<c>takeProfitLimit</c>"] Take Profit Limit.
    /// </summary>
    [Map("takeProfitLimit")] TakeProfitLimit = 5,
}
