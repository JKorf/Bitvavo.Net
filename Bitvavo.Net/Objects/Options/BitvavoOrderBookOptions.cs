using CryptoExchange.Net.Objects.Options;
using System;

namespace Bitvavo.Net.Objects.Options
{
    /// <summary>
    /// Options for the Bitvavo SymbolOrderBook
    /// </summary>
    public class BitvavoOrderBookOptions : OrderBookOptions
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        public static BitvavoOrderBookOptions Default { get; set; } = new BitvavoOrderBookOptions();

        /// <summary>
        /// The top amount of results to keep in sync. If for example limit=10 is used, the order book will contain the 10 best bids and 10 best asks
        /// </summary>
        public int? Limit { get; set; }

        /// <summary>
        /// After how much time we should consider the connection dropped if no data is received for this time after the initial subscriptions
        /// </summary>
        public TimeSpan? InitialDataTimeout { get; set; }

        internal BitvavoOrderBookOptions Copy()
        {
            var result = Copy<BitvavoOrderBookOptions>();
            result.Limit = Limit;
            result.InitialDataTimeout = InitialDataTimeout;
            return result;
        }
    }
}
