using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;

namespace Bitvavo.Net.Objects.Internal;

internal sealed class BitvavoSocketAuthQuery : Query<BitvavoSocketAuthResponse>
{
    public BitvavoSocketAuthQuery(Dictionary<string, object> authPayload)
        : base(authPayload, authenticated: false, weight: 1)
    {
        MessageRouter = MessageRouter.CreateForQuery<BitvavoSocketAuthResponse>(
            "authenticate",
            HandleAuthResponse);
    }

    private CallResult<BitvavoSocketAuthResponse> HandleAuthResponse(SocketConnection connection, System.DateTime receiveTime, string? originalData, BitvavoSocketAuthResponse message)
        => message.Authenticated
                ? CallResult<BitvavoSocketAuthResponse>.Ok(message)
                : CallResult<BitvavoSocketAuthResponse>.Fail(new ServerError(new ErrorInfo(ErrorType.Unauthorized, "Bitvavo WebSocket authentication was rejected by the server")));        
}
