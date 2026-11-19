using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using CryptoExchange.Net.Interfaces.Clients;

namespace Bitvavo.Net.Interfaces.Clients
{
    /// <summary>
    /// Client for accessing the Bitvavo Rest API. 
    /// </summary>
    public interface IBitvavoRestClient : IRestClient<BitvavoCredentials>
    {
        /// <summary>
        /// Exchange API endpoints
        /// </summary>
        /// <see cref="IBitvavoRestClientExchangeApi"/>
        public IBitvavoRestClientExchangeApi ExchangeApi { get; }
    }
}
