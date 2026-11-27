using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Internal;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;

namespace Bitvavo.Net.Objects.Sockets.Subscriptions
{
    internal class BitvavoSubscription<T> : Subscription
    {
        private readonly SocketApiClient _client;
        private readonly BitvavoSocketChannel _channel;
        private readonly string[]? _ticker24hSubscriptionIdentifiers;

        public override string[] SubscriptionIdentifiers => _ticker24hSubscriptionIdentifiers ?? base.SubscriptionIdentifiers;

        public BitvavoSubscription(
            SocketApiClient client,
            ILogger logger,
            string channel,
            string eventName,
            string[] symbols,
            KlineInterval? interval,
            Action<DateTime, string?, T> handler)
            : base(logger, false)
        {
            _client = client;
            _channel = new BitvavoSocketChannel(channel, symbols, interval.HasValue ? [interval.Value] : null);
            Topic = channel;
            IndividualSubscriptionCount = symbols.Length;

            var topicFilters = interval.HasValue
                ? symbols.Select(symbol => BitvavoSocketChannel.GetTopicFilter(symbol, interval.Value))
                : symbols;
            var routes = topicFilters.Select(filter => MessageRoute.CreateForEvent<T>(eventName, filter, HandleMessage));

            if (typeof(T) == typeof(BitvavoTicker24hMessage))
            {
                // Batch messages need an unfiltered route. The wire subscription is scoped to its markets.
                _ticker24hSubscriptionIdentifiers = symbols.Select(symbol => $"{channel}:{symbol}").ToArray();
                routes = routes.Append(MessageRoute.CreateForEvent<T>(eventName, HandleMessage));
            }

            MessageRouter = MessageRouter.Create(routes.ToArray());

            CallResult HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, T message)
            {
                handler(receiveTime, originalData, message);
                return CallResult.Ok();
            }
        }

        protected override Query GetSubQuery(SocketConnection connection)
            => CreateSubscriptionQuery(subscribe: true);

        protected override Query GetUnsubQuery(SocketConnection connection)
            => CreateSubscriptionQuery(subscribe: false);

        private Query CreateSubscriptionQuery(bool subscribe)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("action", subscribe ? "subscribe" : "unsubscribe");
            parameters.Add("channels", new[] { _channel });
            return new BitvavoSocketEventQuery(_client, parameters, subscribe ? "subscribed" : "unsubscribed");
        }
    }
}
