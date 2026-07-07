// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Subaccount Status.
/// </summary>
[JsonConverter(typeof(EnumConverter<SubaccountStatus>))]
public enum SubaccountStatus
{
    /// <summary>
    /// ["<c>open</c>"] Open.
    /// </summary>
    [Map("open")] Open = 0,
    /// <summary>
    /// ["<c>closed</c>"] Closed.
    /// </summary>
    [Map("closed")] Closed = 1,
}
