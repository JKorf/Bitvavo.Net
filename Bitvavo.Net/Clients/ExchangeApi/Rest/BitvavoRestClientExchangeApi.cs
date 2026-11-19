using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using Bitvavo.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using Bitvavo.Net.Clients.MessageHandlers;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    /// <inheritdoc cref="IBitvavoRestClientExchangeApi" />
    internal partial class BitvavoRestClientExchangeApi : RestApiClient<BitvavoEnvironment, BitvavoAuthenticationProvider, BitvavoCredentials>, IBitvavoRestClientExchangeApi
    {
        #region fields 
        private readonly BitvavoRestClientExchangeSharedApi _sharedApi;

        protected override ErrorMapping ErrorMapping => BitvavoErrors.Errors;

        /// <inheritdoc />
        protected override IRestMessageHandler MessageHandler { get; } = new BitvavoRestMessageHandler(BitvavoErrors.Errors);
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IBitvavoRestClientExchangeApiAccount Account { get; }
        /// <inheritdoc />
        public IBitvavoRestClientExchangeApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IBitvavoRestClientExchangeApiTrading Trading { get; }
        /// <inheritdoc />
        public string ExchangeName => "Bitvavo";
        #endregion

        #region constructor/destructor
        internal BitvavoRestClientExchangeApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, BitvavoRestOptions options)
            : base(loggerFactory, BitvavoExchange.Metadata.Id, httpClient, options.Environment.RestClientAddress, options, options.ExchangeOptions)
        {
            Account = new BitvavoRestClientExchangeApiAccount(this);
            ExchangeData = new BitvavoRestClientExchangeApiExchangeData(_logger, this);
            Trading = new BitvavoRestClientExchangeApiTrading(_logger, this);

            _sharedApi = new BitvavoRestClientExchangeSharedApi(this);
        }
        #endregion


        /// <inheritdoc />
        public IBitvavoRestClientExchangeSharedApi SharedApi => _sharedApi;

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(BitvavoExchange._serializerContext);


        /// <inheritdoc />
        protected override BitvavoAuthenticationProvider CreateAuthenticationProvider(BitvavoCredentials credentials)
            => new BitvavoAuthenticationProvider(credentials);

        internal async Task<HttpResult> SendAsync(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<Unit>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        internal async Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        {
            var result = await base.SendAsync<T>(definition, parameters, cancellationToken, null, weight).ConfigureAwait(false);
            return result;
        }

        /// <inheritdoc />
        protected override Task<HttpResult<DateTime>> GetServerTimestampAsync()
            => throw new NotImplementedException();

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null) 
            => BitvavoExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
