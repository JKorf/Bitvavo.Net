// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Transfer Status.
/// </summary>
[JsonConverter(typeof(EnumConverter<TransferStatus>))]
public enum TransferStatus
{
    /// <summary>
    /// ["<c>failed</c>"] Failed.
    /// </summary>
    [Map("failed")] Failed = 0,
    /// <summary>
    /// ["<c>pending</c>"] Pending.
    /// </summary>
    [Map("pending")] Pending = 1,
    /// <summary>
    /// ["<c>completed</c>"] Completed.
    /// </summary>
    [Map("completed")] Completed = 2,
}
