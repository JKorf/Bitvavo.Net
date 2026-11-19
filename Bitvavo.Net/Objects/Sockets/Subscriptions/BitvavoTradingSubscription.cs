using Bitvavo.Net.Objects.Internal;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Bitvavo.Net.Objects.Sockets.Subscriptions
{
    internal class BitvavoTradingSubscription : Subscription
    {
        private readonly SocketApiClient _client;
        private readonly BitvavoSocketChannel _channel;
        private readonly Action<DataEvent<BitvavoOrderUpdate>>? _onOrderMessage;
        private readonly Action<DataEvent<BitvavoUserTradeUpdate>>? _onTradeMessage;

        public bool AllSymbols { get; }

        public BitvavoTradingSubscription(
            SocketApiClient client,
            ILogger logger,
            string[]? symbols,
            Action<DataEvent<BitvavoOrderUpdate>>? onOrderMessage,
            Action<DataEvent<BitvavoUserTradeUpdate>>? onTradeMessage)
            : base(logger, true)
        {
            _client = client;
            _onOrderMessage = onOrderMessage;
            _onTradeMessage = onTradeMessage;
            _channel = new BitvavoSocketChannel("account", (object?)symbols ?? "*");
            Topic = _channel.Name;
            IndividualSubscriptionCount = symbols?.Length ?? 1;
            AllSymbols = symbols == null;

            // Both event types belong to the same wire subscription, regardless of the callbacks requested.
            MessageRouter = symbols == null
                ? MessageRouter.Create(
                    MessageRoute.CreateForEvent<BitvavoOrderUpdate>("order", HandleOrderUpdate),
                    MessageRoute.CreateForEvent<BitvavoUserTradeUpdate>("fill", HandleTradeUpdate))
                : MessageRouter.Create(symbols.SelectMany(symbol => new[]
                {
                    MessageRoute.CreateForEvent<BitvavoOrderUpdate>("order", symbol, HandleOrderUpdate),
                    MessageRoute.CreateForEvent<BitvavoUserTradeUpdate>("fill", symbol, HandleTradeUpdate)
                }).ToArray());
        }

        protected override Query GetSubQuery(SocketConnection connection)
            => CreateSubscriptionQuery(subscribe: true);

        protected override Query GetUnsubQuery(SocketConnection connection)
            => CreateSubscriptionQuery(subscribe: false);

        private CallResult HandleOrderUpdate(SocketConnection connection, DateTime receiveTime, string? originalData, BitvavoOrderUpdate update)
        {
            _onOrderMessage?.Invoke(CreateEvent(update, update.Symbol, update.UpdateTime ?? update.CreateTime, receiveTime, originalData));
            return CallResult.Ok();
        }

        private CallResult HandleTradeUpdate(SocketConnection connection, DateTime receiveTime, string? originalData, BitvavoUserTradeUpdate update)
        {
            _onTradeMessage?.Invoke(CreateEvent(update, update.Symbol, update.TimestampPrecise ?? update.Timestamp, receiveTime, originalData));
            return CallResult.Ok();
        }

        private DataEvent<T> CreateEvent<T>(T update, string symbol, DateTime? timestamp, DateTime receiveTime, string? originalData)
            => new DataEvent<T>(BitvavoExchange.Metadata.Id, update, receiveTime, originalData)
                .WithUpdateType(SocketUpdateType.Update)
                .WithStreamId(_channel.Name)
                .WithSymbol(symbol)
                .WithDataTimestamp(timestamp, _client.GetTimeOffset());

        private Query CreateSubscriptionQuery(bool subscribe)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("action", subscribe ? "subscribe" : "unsubscribe");
            parameters.Add("channels", new[] { _channel });
            return new BitvavoSocketEventQuery(_client, parameters, subscribe ? "subscribed" : "unsubscribed");
        }
    }
}
