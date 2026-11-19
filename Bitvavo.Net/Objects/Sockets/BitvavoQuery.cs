using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using System;
using Bitvavo.Net.Objects.Models;

namespace Bitvavo.Net.Objects.Sockets
{
    internal class BitvavoQuery<T> : Query<T>
    {
        public BitvavoQuery(BitvavoModel request, bool authenticated, int weight = 1) : base(request, authenticated, weight)
        {
            MessageRouter = MessageRouter.CreateForQuery<T>("", HandleMessage);
        }

        public CallResult<T> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, T message)
        {
            return CallResult.Ok(message, originalData);
        }
    }
}
