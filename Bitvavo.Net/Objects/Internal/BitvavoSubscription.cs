using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;

namespace Bitvavo.Net.Objects.Internal;


internal sealed class BitvavoSubscription<T> : Subscription
{
    private readonly Action<DateTime, string?, T> _handler;
    private readonly BitvavoSocketChannel _channel;

    public BitvavoSubscription(
        ILogger logger,
        string typeIdentifier,
        string topic,
        BitvavoSocketChannel channel,
        Action<DateTime, string?, T> handler)
        : base(logger, authenticated: false)
    {
        _handler = handler;
        _channel = channel;

        IndividualSubscriptionCount = channel.Symbols.Length;
        MessageRouter = MessageRouter.CreateForEvent<T>(typeIdentifier, topic, DoHandleMessage);
    }

    public BitvavoSubscription(
        ILogger logger,
        string typeIdentifier,
        string[] topics,
        BitvavoSocketChannel channel,
        Action<DateTime, string?, T> handler)
        : base(logger, authenticated: false)
    {
        _handler = handler;
        _channel = channel;

        IndividualSubscriptionCount = channel.Symbols.Length;
        MessageRouter = MessageRouter.CreateForEvent<T>(typeIdentifier, topics, DoHandleMessage);
    }

    protected override Query? GetSubQuery(SocketConnection connection) =>
        new BitvavoSubscribeQuery(new BitvavoSocketRequest
        {
            Action = "subscribe",
            Channels = new[] { _channel },
        });

    protected override Query? GetUnsubQuery(SocketConnection connection) =>
        new BitvavoSubscribeQuery(new BitvavoSocketRequest
        {
            Action = "unsubscribe",
            Channels = new[] { _channel },
        });

    public CallResult DoHandleMessage(SocketConnection connection, DateTime receiveTime, string? originalData, T message)
    {
        _handler.Invoke(receiveTime, originalData, message);
        return CallResult.Ok();
    }
}
