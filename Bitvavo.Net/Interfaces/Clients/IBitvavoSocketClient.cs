using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using CryptoExchange.Net.Interfaces.Clients;

namespace Bitvavo.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Bitvavo websocket API
    /// </summary>
    public interface IBitvavoSocketClient : ISocketClient<BitvavoCredentials>
    {
        /// <summary>
        /// Exchange API endpoints
        /// </summary>
        /// <see cref="IBitvavoSocketClientExchangeApi"/>
        public IBitvavoSocketClientExchangeApi ExchangeApi { get; }
    }
}
