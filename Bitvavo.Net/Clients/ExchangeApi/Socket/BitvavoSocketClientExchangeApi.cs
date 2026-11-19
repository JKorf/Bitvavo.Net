using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using Bitvavo.Net.Clients.MessageHandlers;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using Bitvavo.Net.Objects.Options;
using Bitvavo.Net.Objects.Sockets;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    /// <summary>
    /// Client providing access to the Bitvavo Exchange websocket Api
    /// </summary>
    internal partial class BitvavoSocketClientExchangeApi : SocketApiClient<BitvavoEnvironment, BitvavoAuthenticationProvider, BitvavoCredentials>, IBitvavoSocketClientExchangeApi
    {
        #region fields
        private readonly BitvavoSocketClientExchangeSharedApi _sharedApi;

        protected override ErrorMapping ErrorMapping => BitvavoErrors.Errors;
        #endregion

        #region Api clients
        /// <inheritdoc />
        public IBitvavoSocketClientExchangeApiAccount Account { get; }
        /// <inheritdoc />
        public IBitvavoSocketClientExchangeApiExchangeData ExchangeData { get; }
        /// <inheritdoc />
        public IBitvavoSocketClientExchangeApiTrading Trading { get; }
        #endregion

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal BitvavoSocketClientExchangeApi(ILoggerFactory? loggerFactory, BitvavoSocketOptions options) :
            base(loggerFactory, BitvavoExchange.Metadata.Id, options.Environment.SocketClientAddress, options, options.ExchangeOptions)
        {
            RateLimiter = BitvavoExchange.RateLimiter.Bitvavo;
            Account = new BitvavoSocketClientExchangeApiAccount();
            ExchangeData = new BitvavoSocketClientExchangeApiExchangeData(this);
            Trading = new BitvavoSocketClientExchangeApiTrading();
            _sharedApi = new BitvavoSocketClientExchangeSharedApi(this);
        }
        #endregion

        /// <inheritdoc />
        protected override IMessageSerializer CreateSerializer() => new SystemTextJsonMessageSerializer(BitvavoExchange._serializerContext);
        /// <inheritdoc />
        public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) => new BitvavoSocketSpotMessageHandler();

        /// <inheritdoc />
        protected override BitvavoAuthenticationProvider CreateAuthenticationProvider(BitvavoCredentials credentials)
            => new BitvavoAuthenticationProvider(credentials);

        /// <inheritdoc />
        public IBitvavoSocketClientExchangeSharedApi SharedApi => _sharedApi;

        internal Task<QueryResult<T>> SendAsync<T>(string action, Parameters? parameters, CancellationToken ct, int weight = 1)
        {
            parameters ??= new Parameters(BitvavoExchange._parameterSerializationSettings);
            var query = new BitvavoQuery<T>(this, action, parameters, false, weight);
            return QueryAsync(BaseAddress.AppendPath("/v2"), query, ct);
        }

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => BitvavoExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
