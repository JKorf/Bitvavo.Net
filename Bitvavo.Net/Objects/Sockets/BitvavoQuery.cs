using Bitvavo.Net.Objects.Internal;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using System;
using System.Globalization;

namespace Bitvavo.Net.Objects.Sockets
{
    internal class BitvavoQuery<T> : Query<T>
    {
        private readonly SocketApiClient _client;

        public BitvavoQuery(SocketApiClient client, string action, Parameters parameters, bool authenticated, int weight)
            : base(parameters, authenticated, weight)
        {
            _client = client;
            parameters.Add("action", action);
            parameters.Add("requestId", Id);

            MessageRouter = MessageRouter.CreateForQuery<BitvavoSocketResponse<T>, T>(Id.ToString(CultureInfo.InvariantCulture), HandleMessage);
        }

        private CallResult<T> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BitvavoSocketResponse<T> message)
        {
            if (message.ErrorCode.HasValue)
                return CallResult.Fail<T>(new ServerError(message.ErrorCode.Value, _client.GetErrorInfo(message.ErrorCode.Value, message.Error)), originalData);

            if (message.Response == null)
                return CallResult.Fail<T>(new DeserializeError("Missing response in the Bitvavo WebSocket reply"), originalData);

            return CallResult.Ok(message.Response, originalData);
        }
    }
}
