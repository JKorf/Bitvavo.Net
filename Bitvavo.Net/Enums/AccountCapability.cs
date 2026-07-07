// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Account Capability.
/// </summary>
[JsonConverter(typeof(EnumConverter<AccountCapability>))]
public enum AccountCapability
{
    /// <summary>
    /// ["<c>buy</c>"] Buy.
    /// </summary>
    [Map("buy")] Buy = 0,
    /// <summary>
    /// ["<c>sell</c>"] Sell.
    /// </summary>
    [Map("sell")] Sell = 1,
    /// <summary>
    /// ["<c>view</c>"] View.
    /// </summary>
    [Map("view")] View = 2,
    /// <summary>
    /// ["<c>withdraw</c>"] Withdraw.
    /// </summary>
    [Map("withdraw")] Withdraw = 3,
}
