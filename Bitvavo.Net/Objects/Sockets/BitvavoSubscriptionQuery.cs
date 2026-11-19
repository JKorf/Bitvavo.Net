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
    internal class BitvavoSubscriptionQuery : Query<BitvavoSubscriptionResponse>
    {
        private readonly SocketApiClient _client;
        private readonly string _expectedEvent;

        public BitvavoSubscriptionQuery(SocketApiClient client, Parameters parameters, string expectedEvent) : base(parameters, false, 1)
        {
            _client = client;
            _expectedEvent = expectedEvent;
            parameters.Add("requestId", Id);

            MessageRouter = MessageRouter.CreateForQuery<BitvavoSubscriptionResponse>(Id.ToString(CultureInfo.InvariantCulture), HandleMessage);
        }

        private CallResult<BitvavoSubscriptionResponse> HandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, BitvavoSubscriptionResponse message)
        {
            if (message.ErrorCode.HasValue)
                return CallResult.Fail<BitvavoSubscriptionResponse>(new ServerError(message.ErrorCode.Value, _client.GetErrorInfo(message.ErrorCode.Value, message.Error)), originalData);

            if (message.Event != _expectedEvent)
                return CallResult.Fail<BitvavoSubscriptionResponse>(new DeserializeError("Unexpected Bitvavo WebSocket subscription response"), originalData);

            return CallResult.Ok(message, originalData);
        }
    }
}
