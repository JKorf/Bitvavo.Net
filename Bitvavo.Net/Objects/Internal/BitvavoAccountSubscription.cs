using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bitvavo.Net.Objects.Internal
{
    internal sealed class BitvavoAccountSubscription<T> : Subscription
    {
        private readonly Action<DateTime, string?, T> _handler;

        public BitvavoAccountSubscription(
            ILogger logger,
            string typeIdentifier,
            Action<DateTime, string?, T> handler)
            : base(logger, authenticated: true)
        {
            _handler = handler;

            MessageRouter = MessageRouter.CreateForEvent<T>(typeIdentifier, DoHandleMessage);
        }

        protected override CryptoExchange.Net.Sockets.Query? GetSubQuery(SocketConnection connection) =>
            new BitvavoSubscribeQuery(new BitvavoSocketRequest
            {
                Action = "subscribe",
                Channels = new[]
                {
                    new BitvavoAllMarketRequest
                    {
                        Name = "account"
                    },
                },
            });

        protected override CryptoExchange.Net.Sockets.Query? GetUnsubQuery(SocketConnection connection) =>
            new BitvavoSubscribeQuery(new BitvavoSocketRequest
            {
                Action = "unsubscribe",
                Channels =
                [
                    new BitvavoAllMarketRequest
                    {
                        Name = "account"
                    }
                ],
            });

        public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, T message)
        {
            _handler.Invoke(receiveTime, originalData, message);
            return CallResult.Ok();
        }
    }
}
