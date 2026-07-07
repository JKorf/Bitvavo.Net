// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Trigger Type.
/// </summary>
[JsonConverter(typeof(EnumConverter<TriggerType>))]
public enum TriggerType
{
    /// <summary>
    /// ["<c>price</c>"] Price.
    /// </summary>
    [Map("price")] Price = 0,
}
