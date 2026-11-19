using CryptoExchange.Net.Interfaces.Clients;
using System;

namespace Bitvavo.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// Bitvavo Exchange WebSocket API
    /// </summary>
    public interface IBitvavoSocketClientExchangeApi : ISocketApiClient<BitvavoCredentials>, IDisposable
    {
        /// <summary>
        /// Account endpoints
        /// </summary>
        IBitvavoSocketClientExchangeApiAccount Account { get; }

        /// <summary>
        /// Exchange data endpoints
        /// </summary>
        IBitvavoSocketClientExchangeApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Trading endpoints
        /// </summary>
        IBitvavoSocketClientExchangeApiTrading Trading { get; }

        /// <summary>
        /// [V2] Gets the aggregate Shared API interface. Shared APIs provide a common,
        /// exchange-independent contract for accessing functionality across different
        /// exchange client libraries.
        /// </summary>
        public IBitvavoSocketClientExchangeSharedApi SharedApi { get; }
    }
}
