// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Self Trade Prevention.
/// </summary>
[JsonConverter(typeof(EnumConverter<SelfTradePrevention>))]
public enum SelfTradePrevention
{
    /// <summary>
    /// ["<c>decrementAndCancel</c>"] Decrement And Cancel.
    /// </summary>
    [Map("decrementAndCancel")] DecrementAndCancel = 0,
    /// <summary>
    /// ["<c>cancelOldest</c>"] Cancel Oldest.
    /// </summary>
    [Map("cancelOldest")] CancelOldest = 1,
    /// <summary>
    /// ["<c>cancelNewest</c>"] Cancel Newest.
    /// </summary>
    [Map("cancelNewest")] CancelNewest = 2,
    /// <summary>
    /// ["<c>cancelBoth</c>"] Cancel Both.
    /// </summary>
    [Map("cancelBoth")] CancelBoth = 3,
}
