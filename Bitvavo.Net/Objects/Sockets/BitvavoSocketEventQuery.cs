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
    internal class BitvavoSocketEventQuery : Query<BitvavoSocketEventResponse>
    {
        private readonly SocketApiClient _client;
        private readonly string _expectedEvent;

        public BitvavoSocketEventQuery(SocketApiClient client, Parameters parameters, string expectedEvent, bool matchByEvent = false) : base(parameters, false, 1)
        {
            _client = client;
            _expectedEvent = expectedEvent;
            parameters.Add("requestId", Id);

            var requestId = Id.ToString(CultureInfo.InvariantCulture);
            MessageRouter = matchByEvent
                ? MessageRouter.CreateForQuery<BitvavoSocketEventResponse>([requestId, expectedEvent], HandleMessage)
                : MessageRouter.CreateForQuery<BitvavoSocketEventResponse>(requestId, HandleMessage);
        }

        private CallResult<BitvavoSocketEventResponse> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BitvavoSocketEventResponse message)
        {
            if (message.ErrorCode.HasValue)
                return CallResult.Fail<BitvavoSocketEventResponse>(new ServerError(message.ErrorCode.Value, _client.GetErrorInfo(message.ErrorCode.Value, message.Error)), originalData);

            if (message.Event != _expectedEvent)
                return CallResult.Fail<BitvavoSocketEventResponse>(new DeserializeError("Unexpected Bitvavo WebSocket event"), originalData);

            return CallResult.Ok(message, originalData);
        }
    }
}
