// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Asset Status.
/// </summary>
[JsonConverter(typeof(EnumConverter<AssetStatus>))]
public enum AssetStatus
{
    /// <summary>
    /// ["<c>OK</c>"] Ok.
    /// </summary>
    [Map("OK")] Ok = 0,
    /// <summary>
    /// ["<c>MAINTENANCE</c>"] Maintenance.
    /// </summary>
    [Map("MAINTENANCE")] Maintenance = 1,
    /// <summary>
    /// ["<c>DELISTED</c>"] Delisted.
    /// </summary>
    [Map("DELISTED")] Delisted = 2,
}
