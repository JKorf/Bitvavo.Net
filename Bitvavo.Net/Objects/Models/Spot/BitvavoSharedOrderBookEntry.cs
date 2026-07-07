// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using CryptoExchange.Net.Interfaces;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Shared Order Book Entry.
/// </summary>
public record BitvavoSharedOrderBookEntry : ISymbolOrderBookEntry
{
    /// <summary>
    /// Price.
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// Quantity.
    /// </summary>
    public decimal Quantity { get; set; }
}
