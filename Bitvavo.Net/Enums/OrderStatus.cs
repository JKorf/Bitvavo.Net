// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Order Status.
/// </summary>
[JsonConverter(typeof(EnumConverter<OrderStatus>))]
public enum OrderStatus
{
    /// <summary>
    /// ["<c>new</c>"] New.
    /// </summary>
    [Map("new")] New = 0,
    /// <summary>
    /// ["<c>awaitingTrigger</c>"] Awaiting Trigger.
    /// </summary>
    [Map("awaitingTrigger")] AwaitingTrigger = 1,
    /// <summary>
    /// ["<c>canceled</c>"] Canceled.
    /// </summary>
    [Map("canceled")] Canceled = 2,
    /// <summary>
    /// ["<c>canceledAuction</c>"] Canceled Auction.
    /// </summary>
    [Map("canceledAuction")] CanceledAuction = 3,
    /// <summary>
    /// ["<c>canceledSelfTradePrevention</c>"] Canceled Self Trade Prevention.
    /// </summary>
    [Map("canceledSelfTradePrevention")] CanceledSelfTradePrevention = 4,
    /// <summary>
    /// ["<c>canceledIOC</c>"] Canceled Ioc.
    /// </summary>
    [Map("canceledIOC")] CanceledIoc = 5,
    /// <summary>
    /// ["<c>canceledFOK</c>"] Canceled Fok.
    /// </summary>
    [Map("canceledFOK")] CanceledFok = 6,
    /// <summary>
    /// ["<c>canceledMarketProtection</c>"] Canceled Market Protection.
    /// </summary>
    [Map("canceledMarketProtection")] CanceledMarketProtection = 7,
    /// <summary>
    /// ["<c>canceledPostOnly</c>"] Canceled Post Only.
    /// </summary>
    [Map("canceledPostOnly")] CanceledPostOnly = 8,
    /// <summary>
    /// ["<c>filled</c>"] Filled.
    /// </summary>
    [Map("filled")] Filled = 9,
    /// <summary>
    /// ["<c>partiallyFilled</c>"] Partially Filled.
    /// </summary>
    [Map("partiallyFilled")] PartiallyFilled = 10,
    /// <summary>
    /// ["<c>expired</c>"] Expired.
    /// </summary>
    [Map("expired")] Expired = 11,
    /// <summary>
    /// ["<c>rejected</c>"] Rejected.
    /// </summary>
    [Map("rejected")] Rejected = 12,
}
