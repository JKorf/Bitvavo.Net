using Bitvavo.Net.Interfaces.Clients.SpotApi;
using Bitvavo.Net.Objects.Internal;
using Bitvavo.Net.Objects.Models.Spot.Streams;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.Sockets.Default;
using CryptoExchange.Net.Sockets.Default.Routing;
using Microsoft.Extensions.Logging;

namespace Bitvavo.Net.Clients.SpotApi;

/// <inheritdoc />
internal sealed class BitvavoSocketClientSpotApiAccount : IBitvavoSocketClientSpotApiAccount
{
    private readonly ILogger _logger;
    private readonly BitvavoSocketClientSpotApi _client;

    internal BitvavoSocketClientSpotApiAccount(ILogger logger, BitvavoSocketClientSpotApi client)
    {
        _logger = logger;
        _client = client;
    }

    /// <inheritdoc />
    public Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(
        Action<DataEvent<BitvavoStreamOrderUpdate>> onMessage,
        CancellationToken ct = default)
        => SubscribeAccountAsync<BitvavoStreamOrderUpdate>(
            typeIdentifier: "order",
            onMessage: msg => onMessage(msg),
            symbolSelector: m => m.Symbol,
            ct: ct);

    /// <inheritdoc />
    public Task<WebSocketResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(
        Action<DataEvent<BitvavoStreamFillEvent>> onMessage,
        CancellationToken ct = default)
        => SubscribeAccountAsync<BitvavoStreamFillEvent>(
            typeIdentifier: "fill",
            onMessage: msg => onMessage(msg),
            symbolSelector: m => m.Symbol,
            ct: ct);

    private Task<WebSocketResult<UpdateSubscription>> SubscribeAccountAsync<T>(
        string typeIdentifier,
        Action<DataEvent<T>> onMessage,
        Func<T, string> symbolSelector,
        CancellationToken ct)
    {
        var subscription = new BitvavoAccountSubscription<T>(
            _logger,
            typeIdentifier: typeIdentifier,
            handler: (receiveTime, originalData, message) =>
                onMessage(new DataEvent<T>(
                    BitvavoExchange.ExchangeName,
                    message,
                    receiveTime,
                    originalData)
                    .WithSymbol(symbolSelector(message))
                    .WithStreamId("account")));

        return _client.SubscribeInternalAsync(_client.BaseAddress, subscription, ct);
    }
}
