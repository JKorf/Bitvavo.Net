using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.OrderBook;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using Bitvavo.Net.Interfaces;
using Bitvavo.Net.Interfaces.Clients;
using Bitvavo.Net.Objects.Options;

namespace Bitvavo.Net.SymbolOrderBooks
{
    /// <summary>
    /// Bitvavo order book factory
    /// </summary>
    public class BitvavoOrderBookFactory : IBitvavoOrderBookFactory
    {
        private readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public BitvavoOrderBookFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            
                        Exchange = new OrderBookFactory<BitvavoOrderBookOptions>(CreateExchange, Create);

        }

                 /// <inheritdoc />
        public IOrderBookFactory<BitvavoOrderBookOptions> Exchange { get; }


        /// <inheritdoc />
        public ISymbolOrderBook Create(SharedSymbol symbol, Action<BitvavoOrderBookOptions>? options = null)
        {
            var symbolName = symbol.GetSymbol(BitvavoExchange.FormatSymbol);
            throw new NotImplementedException();

#warning TODO
            //if (symbol.TradingMode == TradingMode.Spot)
            //    return CreateSpot(symbolName, options);

            //return Create(symbolName, options);
        }

                 /// <inheritdoc />
        public ISymbolOrderBook CreateExchange(string symbol, Action<BitvavoOrderBookOptions>? options = null)
            => new BitvavoExchangeSymbolOrderBook(symbol, options, 
                                                          _serviceProvider.GetRequiredService<ILoggerFactory>(),
                                                          _serviceProvider.GetRequiredService<IBitvavoRestClient>(),
                                                          _serviceProvider.GetRequiredService<IBitvavoSocketClient>());


    }
}
