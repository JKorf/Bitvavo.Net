using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using CryptoExchange.Net.SharedApis;

namespace Bitvavo.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for the shared REST and WebSocket API implementations of Bitvavo
    /// </summary>
    public interface IBitvavoSharedApiClient : ISharedApiClientBase
    {
        /// <summary>
        /// Exchange REST Shared API implementations
        /// </summary>
        IBitvavoRestClientExchangeSharedApi ExchangeRest { get; }

        /// <summary>
        /// Exchange WebSocket Shared API implementations
        /// </summary>
        IBitvavoSocketClientExchangeSharedApi ExchangeSocket { get; }
    }
}
