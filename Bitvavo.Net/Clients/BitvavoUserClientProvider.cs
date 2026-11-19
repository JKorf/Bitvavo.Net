using Bitvavo.Net.Interfaces.Clients;
using Bitvavo.Net.Objects.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Concurrent;
using System.Net.Http;
using CryptoExchange.Net.Clients;

namespace Bitvavo.Net.Clients
{
    /// <inheritdoc />
    public class BitvavoUserClientProvider : UserClientProvider<
        IBitvavoRestClient,
        IBitvavoSocketClient,
        BitvavoRestOptions,
        BitvavoSocketOptions,
        BitvavoCredentials,
        BitvavoEnvironment
        >, IBitvavoUserClientProvider
    {
        private readonly IOptions<BitvavoRestOptions> _restOptions;

        /// <inheritdoc />
        public override string ExchangeName => BitvavoExchange.Metadata.Id;

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="optionsDelegate">Options to use for created clients</param>
        public BitvavoUserClientProvider(Action<BitvavoOptions>? optionsDelegate = null)
            : this(null, null, Options.Create(ApplyOptionsDelegate(optionsDelegate).Rest), Options.Create(ApplyOptionsDelegate(optionsDelegate).Socket))
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        public BitvavoUserClientProvider(
            HttpClient? httpClient,
            ILoggerFactory? loggerFactory,
            IOptions<BitvavoRestOptions> restOptions,
            IOptions<BitvavoSocketOptions> socketOptions)
            : base(httpClient, loggerFactory, restOptions, socketOptions)
        {
            _restOptions = restOptions;
        }

        /// <inheritdoc />
        protected override IBitvavoRestClient ConstructRestClient(HttpClient client, ILoggerFactory? loggerFactory, IOptions<BitvavoRestOptions> options)
        {
            if (!ReferenceEquals(options, _restOptions))
                options.Value.OperatorId = _restOptions.Value.OperatorId;

            return new BitvavoRestClient(client, loggerFactory, options);
        }
        /// <inheritdoc />
        protected override IBitvavoSocketClient ConstructSocketClient(ILoggerFactory? loggerFactory, IOptions<BitvavoSocketOptions> options)
            => new BitvavoSocketClient(options, loggerFactory);
    }
}
