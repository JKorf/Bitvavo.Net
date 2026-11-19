using Microsoft.Extensions.Logging;
using System.Net.Http;
using System;
using Bitvavo.Net.Interfaces.Clients;
using Bitvavo.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Options;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using Bitvavo.Net.Clients.ExchangeApi;

namespace Bitvavo.Net.Clients
{
    /// <inheritdoc cref="IBitvavoRestClient" />
    public class BitvavoRestClient : BaseRestClient<BitvavoEnvironment, BitvavoCredentials>, IBitvavoRestClient
    {
        #region Api clients

        /// <inheritdoc />
        public IBitvavoRestClientExchangeApi ExchangeApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of the BitvavoRestClient using provided options
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BitvavoRestClient(Action<BitvavoRestOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate)))
        {
        }

        /// <summary>
        /// Create a new instance of the BitvavoRestClient using provided options
        /// </summary>
        /// <param name="options">Option configuration</param>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="httpClient">Http client for this client</param>
        public BitvavoRestClient(HttpClient? httpClient, ILoggerFactory? loggerFactory, IOptions<BitvavoRestOptions> options) : base(loggerFactory, "Bitvavo")
        {
            Initialize(options.Value);

            ExchangeApi = AddApiClient(new BitvavoRestClientExchangeApi(loggerFactory, httpClient, options.Value));
        }

        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<BitvavoRestOptions> optionsDelegate)
        {
            BitvavoRestOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }
    }
}
