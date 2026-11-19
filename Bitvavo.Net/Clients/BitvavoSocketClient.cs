using CryptoExchange.Net.Clients;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Bitvavo.Net.Interfaces.Clients;
using Bitvavo.Net.Objects.Options;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using Bitvavo.Net.Clients.ExchangeApi;

namespace Bitvavo.Net.Clients
{
    /// <inheritdoc cref="IBitvavoSocketClient" />
    public class BitvavoSocketClient : BaseSocketClient<BitvavoEnvironment, BitvavoCredentials>, IBitvavoSocketClient
    {
        #region fields
        #endregion

        #region Api clients

        /// <inheritdoc />
        public IBitvavoSocketClientExchangeApi ExchangeApi { get; }

        #endregion

        #region constructor/destructor

        /// <summary>
        /// Create a new instance of BitvavoSocketClient
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BitvavoSocketClient(Action<BitvavoSocketOptions>? optionsDelegate = null)
            : this(Options.Create(ApplyOptionsDelegate(optionsDelegate)), null)
        {
        }

        /// <summary>
        /// Create a new instance of BitvavoSocketClient
        /// </summary>
        /// <param name="loggerFactory">The logger factory</param>
        /// <param name="options">Option configuration</param>
        public BitvavoSocketClient(IOptions<BitvavoSocketOptions> options, ILoggerFactory? loggerFactory = null) : base(loggerFactory, "Bitvavo")
        {
            Initialize(options.Value);

            ExchangeApi = AddApiClient(new BitvavoSocketClientExchangeApi(loggerFactory, options.Value));
        }
        #endregion

        /// <summary>
        /// Set the default options to be used when creating new clients
        /// </summary>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public static void SetDefaultOptions(Action<BitvavoSocketOptions> optionsDelegate)
        {
            BitvavoSocketOptions.Default = ApplyOptionsDelegate(optionsDelegate);
        }
    }
}
