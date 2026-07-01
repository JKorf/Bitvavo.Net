using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Order book entry
/// </summary>
[JsonConverter(typeof(ArrayConverter<BitvavoOrderBookEntry>))]
public record BitvavoOrderBookEntry : ISymbolOrderBookEntry
{
    /// <summary>
    /// Price level (quote-asset units).
    /// </summary>
    [ArrayProperty(0)]
    public decimal Price { get; set; }

    /// <summary>
    /// Aggregate size at the price level (base-asset units).
    /// </summary>
    [ArrayProperty(1)]
    public decimal Quantity { get; set; }
}
