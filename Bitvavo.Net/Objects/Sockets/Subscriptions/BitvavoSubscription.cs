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
        private readonly Action<DateTime, string?, T> _handler;

        public BitvavoSubscription(SocketApiClient client, ILogger logger, BitvavoSocketChannel channel, string eventName, Action<DateTime, string?, T> handler)
            : base(logger, false)
        {
            _client = client;
            _channel = channel;
            _handler = handler;
            Topic = channel.Name;
            IndividualSubscriptionCount = channel.Symbols.Length;
            var topicFilters = channel.Intervals == null
                ? channel.Symbols
                : channel.Symbols.SelectMany(symbol => channel.Intervals.Select(interval => BitvavoSocketChannel.GetTopicFilter(symbol, interval)));
            var routes = topicFilters.Select(filter => MessageRoute.CreateForEvent<T>(eventName, filter, HandleMessage));

            // A 24-hour ticker message can contain several symbols, so filter its items in the callback.
            // Keep per-symbol routes so the framework can distinguish subscriptions on the same connection.
            if (typeof(T) == typeof(BitvavoTicker24hMessage))
                routes = routes.Append(MessageRoute.CreateForEvent<T>(eventName, HandleMessage));

            MessageRouter = MessageRouter.Create(routes.ToArray());
        }

        protected override Query GetSubQuery(SocketConnection connection) => CreateQuery("subscribe", "subscribed");

        protected override Query GetUnsubQuery(SocketConnection connection) => CreateQuery("unsubscribe", "unsubscribed");

        private Query CreateQuery(string action, string expectedEvent)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("action", action);
            parameters.Add("channels", new[] { _channel });
            return new BitvavoSocketEventQuery(_client, parameters, expectedEvent);
        }

        private CallResult HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, T message)
        {
            _handler(receiveTime, originalData, message);
            return CallResult.Ok();
        }
    }
}
