// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Order Book Entry.
/// </summary>
[JsonConverter(typeof(ArrayConverter<BitvavoOrderBookEntry>))]
public record BitvavoOrderBookEntry : ISymbolOrderBookEntry
{
    /// <summary>
    /// Price.
    /// </summary>
    [ArrayProperty(0)]
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity.
    /// </summary>
    [ArrayProperty(1)]
    public decimal Quantity { get; set; }
}
