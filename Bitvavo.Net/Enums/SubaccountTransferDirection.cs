// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Subaccount Transfer Direction.
/// </summary>
[JsonConverter(typeof(EnumConverter<SubaccountTransferDirection>))]
public enum SubaccountTransferDirection
{
    /// <summary>
    /// ["<c>masterToSub</c>"] Master To Sub.
    /// </summary>
    [Map("masterToSub")] MasterToSub = 0,
    /// <summary>
    /// ["<c>subToMaster</c>"] Sub To Master.
    /// </summary>
    [Map("subToMaster")] SubToMaster = 1,
}
