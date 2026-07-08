using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models.Spot;
using Bitvavo.Net.Objects.Models.Spot.Streams;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;

namespace Bitvavo.Net.Interfaces.Clients.SpotApi;

/// <summary>
/// Bitvavo public-stream WebSocket subscriptions. No authentication required.
/// </summary>
public interface IBitvavoSocketClientSpotApiExchangeData
{
    /// <summary>
    /// Subscribe to candle (kline) updates for a symbol and interval
    /// <para>
    /// <a href="https://docs.bitvavo.com/docs/websocket-overview/">Bitvavo WS docs</a>
    /// </para>
    /// </summary>
    /// <param name="symbol">Symbol identifier, e.g. <c>"ETH-EUR"</c>.</param>
    /// <param name="interval">Candle interval.</param>
    /// <param name="onMessage">Handler invoked for each candle event.</param>
    /// <param name="ct">Cancellation token used to close this subscription.</param>
    Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(
        string symbol,
        KlineInterval interval,
        Action<DataEvent<BitvavoStreamCandleEvent>> onMessage,
        CancellationToken ct = default);

    /// <summary>
    /// Subscribe to public trade updates for a symbol
    /// <para>
    /// <a href="https://docs.bitvavo.com/docs/websocket-overview/">Bitvavo WS docs</a>
    /// </para>
    /// </summary>
    /// <param name="symbol">Symbol identifier, e.g. <c>"BTC-EUR"</c>.</param>
    /// <param name="onMessage">Handler invoked for each public trade event</param>
    /// <param name="ct">Cancellation token used to close this subscription.</param>
    Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
        string symbol,
        Action<DataEvent<BitvavoStreamTrade>> onMessage,
        CancellationToken ct = default);

    /// <summary>
    /// Subscribe to public trade updates for multiple symbols
    /// <para>
    /// <a href="https://docs.bitvavo.com/docs/websocket-overview/">Bitvavo WS docs</a>
    /// </para>
    /// </summary>
    /// <param name="symbols">Symbol identifiers, e.g. <c>["BTC-EUR", "ETH-EUR"]</c>.</param>
    /// <param name="onMessage">Handler invoked for each public trade event</param>
    /// <param name="ct">Cancellation token used to close this subscription.</param>
    Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
        IEnumerable<string> symbols,
        Action<DataEvent<BitvavoStreamTrade>> onMessage,
        CancellationToken ct = default);

    /// <summary>
    /// Subscribe to public order book updates for multiple symbols
    /// <para>
    /// <a href="https://docs.bitvavo.com/docs/websocket-overview/">Bitvavo WS docs</a>
    /// </para>
    /// </summary>
    /// <param name="symbols">Symbol identifiers, e.g. <c>["BTC-EUR", "ETH-EUR"]</c>.</param>
    /// <param name="onMessage">Handler invoked for each public trade event</param>
    /// <param name="ct">Cancellation token used to close this subscription.</param>
    Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
        IEnumerable<string> symbols,
        Action<DataEvent<BitvavoOrderBookUpdate>> onMessage,
        CancellationToken ct = default);
}
