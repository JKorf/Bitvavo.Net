// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Missing Price Status.
/// </summary>
[JsonConverter(typeof(EnumConverter<MissingPriceStatus>))]
public enum MissingPriceStatus
{
    /// <summary>
    /// ["<c>PNDG</c>"] Pending.
    /// </summary>
    [Map("PNDG")] Pending = 0,
    /// <summary>
    /// ["<c>NOAP</c>"] Not Applicable.
    /// </summary>
    [Map("NOAP")] NotApplicable = 1,
}
