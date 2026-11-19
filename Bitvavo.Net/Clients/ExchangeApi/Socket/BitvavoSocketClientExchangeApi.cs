using System;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using Bitvavo.Net.Clients.MessageHandlers;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using Bitvavo.Net.Objects.Models;
using Bitvavo.Net.Objects.Options;
using Bitvavo.Net.Objects.Sockets.Subscriptions;

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

        #region constructor/destructor

        /// <summary>
        /// ctor
        /// </summary>
        internal BitvavoSocketClientExchangeApi(ILoggerFactory? loggerFactory, BitvavoSocketOptions options) :
            base(loggerFactory, BitvavoExchange.Metadata.Id, options.Environment.SocketClientAddress!, options, options.ExchangeOptions)
        {
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

        /// <inheritdoc />
        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToXXXUpdatesAsync(Action<DataEvent<BitvavoModel>> onMessage, CancellationToken ct = default)
        {
            var internalHandler = new Action<DateTime, string?, BitvavoModel>((receiveTime, originalData, data) =>
            {
                //UpdateTimeOffset(data.Timestamp);

                onMessage(
                    new DataEvent<BitvavoModel>(BitvavoExchange.Metadata.Id, data, receiveTime, originalData)
                        .WithUpdateType(SocketUpdateType.Update)
                        //.WithStreamId(data.Stream)
                        //.WithSymbol(data.Symbol)
                        //.WithDataTimestamp(data.EventTime, GetTimeOffset())
                    );
            });

            var subscription = new BitvavoSubscription<BitvavoModel>(_logger, new [] { "XXX" }, internalHandler, false);
            return await SubscribeAsync(subscription, ct).ConfigureAwait(false);
        }

        /// <inheritdoc />
        public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
            => BitvavoExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);
    }
}
