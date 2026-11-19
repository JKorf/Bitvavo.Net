using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using System;
using Bitvavo.Net.Objects.Options;

namespace Bitvavo.Net.Interfaces
{
    /// <summary>
    /// Bitvavo local order book factory
    /// </summary>
    public interface IBitvavoOrderBookFactory
    {
        /// <summary>
        /// Exchange order book factory methods
        /// </summary>
        IOrderBookFactory<BitvavoOrderBookOptions> Exchange { get; }

        /// <summary>
        /// Create a SymbolOrderBook for the symbol
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        ISymbolOrderBook Create(SharedSymbol symbol, Action<BitvavoOrderBookOptions>? options = null);

        /// <summary>
        /// Create a new Exchange local order book instance
        /// </summary>
        ISymbolOrderBook Create(string symbol, Action<BitvavoOrderBookOptions>? options = null);

    }
}