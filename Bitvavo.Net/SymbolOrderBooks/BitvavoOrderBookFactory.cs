using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.OrderBook;
using CryptoExchange.Net.SharedApis;
using Bitvavo.Net.Interfaces;
using Bitvavo.Net.Interfaces.Clients;
using Bitvavo.Net.Objects.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;

namespace Bitvavo.Net.SymbolOrderBooks
{
    /// <summary>
    /// Bitvavo order book factory
    /// </summary>
    public class BitvavoOrderBookFactory// : IBitvavoOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <inheritdoc />
        public string ExchangeName => BitvavoExchange.ExchangeName;

        /// <inheritdoc />
        public IOrderBookFactory<BitvavoOrderBookOptions> Spot { get; }


        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public BitvavoOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            Spot = new OrderBookFactory<BitvavoOrderBookOptions>(CreateSpot, Create);
        }

        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<BitvavoOrderBookOptions>? options = null)
        {
            var symbolName = symbol.GetSymbol(BitvavoExchange.FormatSymbol);
            return CreateSpot(symbolName, options);
        }

        /// <summary>
        /// Create a spot SymbolOrderBook
        /// </summary>
        /// <param name="symbol">The symbol</param>
        /// <param name="options">Book options</param>
        /// <returns></returns>
        public ISymbolOrderBook CreateSpot(string symbol, Action<BitvavoOrderBookOptions>? options = null)
            => new BitvavoSpotSymbolOrderBook(symbol,
                                             options,
                                             _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                             _serviceProvider.GetRequiredService<IBitvavoRestClient>(),
                                             _serviceProvider.GetRequiredService<IBitvavoSocketClient>());
    }
}
