using System;
using System.Linq;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Interfaces;
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
            Account = new BitvavoSocketClientExchangeApiAccount(this);
            ExchangeData = new BitvavoSocketClientExchangeApiExchangeData(_logger, this);
            Trading = new BitvavoSocketClientExchangeApiTrading(this, options.OperatorId);
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

        protected override bool ConnectionCanBeUsedFor(SocketConnection connection, string address, bool authenticated, IMessageProcessor? processor, string? topic = null)
        {
            if (!base.ConnectionCanBeUsedFor(connection, address, authenticated, processor, topic))
                return false;

            // CryptoExchange.Net handles identical route sets. Isolate partial overlaps so closing a batch
            // cannot unsubscribe a symbol that another batch on the same socket still needs.
            return processor is not Subscription subscription || !connection.Subscriptions.Any(existing =>
            {
                var existingRoutes = existing.MessageRouter.Routes;
                var incomingRouter = subscription.MessageRouter;
                if (existingRoutes.Length == incomingRouter.Routes.Length && existingRoutes.All(incomingRouter.ContainsCheck))
                    return false;

                return existingRoutes.Any(route => route.TopicFilter != null && incomingRouter.ContainsCheck(route));
            });
        }

        internal Task<WebSocketResult<UpdateSubscription>> SubscribeToUpdatesAsync(Subscription subscription, CancellationToken ct)
            => SubscribeAsync(BaseAddress.AppendPath("/v2"), subscription, ct);

        internal Task<QueryResult<T>> SendPrivateAsync<T>(string action, Parameters? parameters, CancellationToken ct, int weight = 1)
            => SendAsync<T>(action, parameters, ct, weight, authenticated: true);

        internal Task<QueryResult<T>> SendAsync<T>(string action, Parameters? parameters, CancellationToken ct, int weight = 1, bool authenticated = false)
        {
            if (authenticated && AuthenticationProvider == null)
                return Task.FromResult(QueryResult.Fail<T>(Exchange, new NoApiCredentialsError()));

            parameters ??= new Parameters(BitvavoExchange._parameterSerializationSettings);
            var query = new BitvavoQuery<T>(this, action, parameters, authenticated, weight);
            return QueryAsync(BaseAddress.AppendPath("/v2"), query, ct);
        }

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => BitvavoExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
