using CryptoExchange.Net.Interfaces.Clients;
using System;

namespace Bitvavo.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// Bitvavo Exchange API endpoints
    /// </summary>
    public interface IBitvavoRestClientExchangeApi : IRestApiClient<BitvavoCredentials>, IDisposable
    {
        /// <summary>
        /// Endpoints related to account settings, info or actions
        /// </summary>
        /// <see cref="IBitvavoRestClientExchangeApiAccount" />
        public IBitvavoRestClientExchangeApiAccount Account { get; }

        /// <summary>
        /// Endpoints related to retrieving market and system data
        /// </summary>
        /// <see cref="IBitvavoRestClientExchangeApiExchangeData" />
        public IBitvavoRestClientExchangeApiExchangeData ExchangeData { get; }

        /// <summary>
        /// Endpoints related to orders and trades
        /// </summary>
        /// <see cref="IBitvavoRestClientExchangeApiTrading" />
        public IBitvavoRestClientExchangeApiTrading Trading { get; }

        /// <summary>
        /// [V2] Gets the aggregate Shared API interface. Shared APIs provide a common,
        /// exchange-independent contract for accessing functionality across different
        /// exchange client libraries.
        /// </summary>
        public IBitvavoRestClientExchangeSharedApi SharedApi { get; }
    }
}
