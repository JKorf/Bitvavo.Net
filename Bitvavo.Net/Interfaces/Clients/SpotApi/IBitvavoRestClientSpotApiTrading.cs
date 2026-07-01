using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Interfaces.Clients.SpotApi;

/// <summary>
/// Bitvavo signed trading REST endpoints â€” place / update / cancel / inspect orders +
/// retrieve own-trade history. Requires API credentials with the trading capability.
/// Mirrors KrakenRestClientSpotApi.Trading / BinanceRestClientSpotApiTrading.
/// </summary>
public interface IBitvavoRestClientSpotApiTrading
{
    /// <summary>
    /// Place a new order.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/create-order">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="side">Buy or sell. Required.</param>
    /// <param name="orderType">Market / limit / stop-loss / take-profit family. Required.</param>
    /// <param name="operatorId">Account-scoped integer identifying the originator of the order request. Required.</param>
    /// <param name="amount">Quantity in base-asset units. Mutually exclusive with <paramref name="amountQuote"/>.</param>
    /// <param name="amountQuote">Quote-asset budget for market buys. Mutually exclusive with <paramref name="amount"/>.</param>
    /// <param name="price">Limit price.</param>
    /// <param name="triggerAmount">Trigger price for stop-loss / take-profit orders.</param>
    /// <param name="triggerType">Type of trigger event.</param>
    /// <param name="triggerReference">Which price reference Bitvavo evaluates the trigger against.</param>
    /// <param name="timeInForce">Time-in-force policy.</param>
    /// <param name="postOnly">If true, the order is rejected if it would have crossed the spread.</param>
    /// <param name="selfTradePrevention">How to resolve fills against this account's other orders.</param>
    /// <param name="responseRequired">If false, Bitvavo returns a slimmer ack-only response.</param>
    /// <param name="clientOrderId">Caller-assigned UUID for client-side correlation.</param>
    /// <param name="codGroupId">Cancel-on-disconnect group id.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoOrder>> PlaceOrderAsync(
        string symbol,
        OrderSide side,
        OrderType orderType,
        long operatorId,
        decimal? amount = null,
        decimal? amountQuote = null,
        decimal? price = null,
        decimal? triggerAmount = null,
        TriggerType? triggerType = null,
        TriggerReference? triggerReference = null,
        TimeInForce? timeInForce = null,
        bool? postOnly = null,
        SelfTradePrevention? selfTradePrevention = null,
        bool? responseRequired = null,
        string? clientOrderId = null,
        int? codGroupId = null,
        CancellationToken ct = default);

    /// <summary>
    /// Update a resting limit / trigger order. Either <paramref name="orderId"/>
    /// or <paramref name="clientOrderId"/> must be set.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/update-order">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="operatorId">Account-scoped integer identifying the request originator. Required.</param>
    /// <param name="orderId">Server-assigned order id. Set this OR <paramref name="clientOrderId"/>.</param>
    /// <param name="clientOrderId">Client-assigned order id. Set this OR <paramref name="orderId"/>.</param>
    /// <param name="amount">New base-asset quantity.</param>
    /// <param name="amountQuote">New quote-asset budget.</param>
    /// <param name="price">New limit price.</param>
    /// <param name="triggerAmount">New trigger price for stop / take-profit orders.</param>
    /// <param name="timeInForce">New time-in-force policy.</param>
    /// <param name="selfTradePrevention">New self-trade-prevention rule.</param>
    /// <param name="postOnly">New post-only flag.</param>
    /// <param name="responseRequired">If false, Bitvavo returns a slimmer ack-only response.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoOrder>> UpdateOrderAsync(
        string symbol,
        long operatorId,
        string? orderId = null,
        string? clientOrderId = null,
        decimal? amount = null,
        decimal? amountQuote = null,
        decimal? price = null,
        decimal? triggerAmount = null,
        TimeInForce? timeInForce = null,
        SelfTradePrevention? selfTradePrevention = null,
        bool? postOnly = null,
        bool? responseRequired = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get the current state of a single order.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-order">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="orderId">Server-assigned order id. Set this OR <paramref name="clientOrderId"/>.</param>
    /// <param name="clientOrderId">Client-assigned order id. Set this OR <paramref name="orderId"/>.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoOrder>> GetOrderAsync(string symbol, string? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel a single order.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/cancel-order">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="operatorId">Account-scoped originator id. Required by Bitvavo on every order operation.</param>
    /// <param name="orderId">Server-assigned order id. Set this OR <paramref name="clientOrderId"/>.</param>
    /// <param name="clientOrderId">Client-assigned order id. Set this OR <paramref name="orderId"/>.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoOrderId>> CancelOrderAsync(string symbol, long operatorId, string? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel all open orders, optionally scoped to a single market.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/cancel-orders">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="operatorId">Account-scoped originator id. Required by Bitvavo on every order operation (mandatory since API v2.9.0).</param>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<IEnumerable<BitvavoOrderId>>> CancelOrdersAsync(long operatorId, string? symbol = null, CancellationToken ct = default);

    /// <summary>
    /// List open orders.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-open-orders">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="baseAsset">Optional base-asset filter (e.g. <c>"ETH"</c>) â€” returns open orders across every market for that base.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<IEnumerable<BitvavoOrder>>> GetOpenOrdersAsync(string? symbol = null, string? baseAsset = null, CancellationToken ct = default);

    /// <summary>
    /// List historical orders (any state) for a single market.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-orders">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="limit">Maximum entries (1â€“1000, default 500).</param>
    /// <param name="startTime">Inclusive UTC lower bound.</param>
    /// <param name="endTime">Inclusive UTC upper bound.</param>
    /// <param name="orderIdFrom">Pagination cursor â€” only return orders with id &gt;= this value.</param>
    /// <param name="orderIdTo">Pagination cursor â€” only return orders with id &lt;= this value.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<IEnumerable<BitvavoOrder>>> GetOrderHistoryAsync(
        string symbol,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? orderIdFrom = null,
        string? orderIdTo = null,
        CancellationToken ct = default);

    /// <summary>
    /// List the user's own trade fills for a single market.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-trade-history">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="limit">Maximum entries, max 1000</param>
    /// <param name="startTime">Inclusive UTC lower bound.</param>
    /// <param name="endTime">Inclusive UTC upper bound.</param>
    /// <param name="tradeIdFrom">Pagination cursor â€” only return fills with id &gt;= this value.</param>
    /// <param name="tradeIdTo">Pagination cursor â€” only return fills with id &lt;= this value.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<IEnumerable<BitvavoFill>>> GetUserTradesAsync(
        string symbol,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? tradeIdFrom = null,
        string? tradeIdTo = null,
        CancellationToken ct = default);
}
