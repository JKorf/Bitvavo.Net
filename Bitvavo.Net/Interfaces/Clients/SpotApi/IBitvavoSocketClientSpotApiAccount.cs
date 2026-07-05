using Bitvavo.Net.Objects.Models.Spot.Streams;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;

namespace Bitvavo.Net.Interfaces.Clients.SpotApi;

/// <summary>
/// Bitvavo signed WebSocket subscriptions
/// </summary>
public interface IBitvavoSocketClientSpotApiAccount
{
    /// <summary>
    /// Subscribe to user order updates
    /// <para><a href="https://docs.bitvavo.com/docs/websocket-api/track-your-orders">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="onMessage">Data handler</param>
    /// <param name="ct">Cancellation token.</param>
    Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderUpdatesAsync(
        Action<DataEvent<BitvavoStreamOrderUpdate>> onMessage,
        CancellationToken ct = default);

    /// <summary>
    /// Subscribe to user trade updates
    /// <para><a href="https://docs.bitvavo.com/docs/websocket-api/track-your-orders">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="onMessage">Data handler</param>
    /// <param name="ct">Cancellation token.</param>
    Task<WebSocketResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(
        Action<DataEvent<BitvavoStreamFillEvent>> onMessage,
        CancellationToken ct = default);
}
