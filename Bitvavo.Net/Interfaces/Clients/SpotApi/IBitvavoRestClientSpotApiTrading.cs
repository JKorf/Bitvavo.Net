using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Interfaces.Clients.SpotApi;

/// <summary>
/// Order and trading endpoints
/// </summary>
public interface IBitvavoRestClientSpotApiTrading
{
    /// <summary>
    /// Place a new order.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/create-order">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="side">Order side</param>
    /// <param name="orderType">Order type</param>
    /// <param name="operatorId">Originator id</param>
    /// <param name="quantity">Quantity in base-asset units. Mutually exclusive with <paramref name="quoteQuantity"/>.</param>
    /// <param name="quoteQuantity">Quote-asset budget for market buys. Mutually exclusive with <paramref name="quantity"/>.</param>
    /// <param name="price">Limit price.</param>
    /// <param name="triggerQuantity">Trigger price for stop-loss / take-profit orders.</param>
    /// <param name="triggerType">Type of trigger event.</param>
    /// <param name="triggerReference">Trigger price</param>
    /// <param name="timeInForce">Time-in-force.</param>
    /// <param name="postOnly">Post only flag</param>
    /// <param name="selfTradePrevention">Self trade prevention mode</param>
    /// <param name="responseRequired">If false, Returns a slimmer ack-only response.</param>
    /// <param name="clientOrderId">Client order id</param>
    /// <param name="codGroupId">Cancel-on-disconnect group id.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoOrder>> PlaceOrderAsync(
        string symbol,
        OrderSide side,
        OrderType orderType,
        long operatorId,
        decimal? quantity = null,
        decimal? quoteQuantity = null,
        decimal? price = null,
        decimal? triggerQuantity = null,
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
    /// <param name="operatorId">Originator id</param>
    /// <param name="orderId">Order id. Set this OR <paramref name="clientOrderId"/>.</param>
    /// <param name="clientOrderId">Client order id. Set this OR <paramref name="orderId"/>.</param>
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
    /// <param name="orderId">Order id. Set this OR <paramref name="clientOrderId"/>.</param>
    /// <param name="clientOrderId">Client order id. Set this OR <paramref name="orderId"/>.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoOrder>> GetOrderAsync(string symbol, string? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel a single order.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/cancel-order">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="operatorId">Originator id</param>
    /// <param name="orderId">Order id. Set this OR <paramref name="clientOrderId"/>.</param>
    /// <param name="clientOrderId">Client order id. Set this OR <paramref name="orderId"/>.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoOrderId>> CancelOrderAsync(string symbol, long operatorId, string? orderId = null, string? clientOrderId = null, CancellationToken ct = default);

    /// <summary>
    /// Cancel all open orders
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/cancel-orders">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="operatorId">Originator id</param>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoOrderId[]>> CancelOrdersAsync(long operatorId, string? symbol = null, CancellationToken ct = default);

    /// <summary>
    /// Get open orders
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-open-orders">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="baseAsset">Filter by base asset name, for example `ETH`.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoOrder[]>> GetOpenOrdersAsync(string? symbol = null, string? baseAsset = null, CancellationToken ct = default);

    /// <summary>
    /// Get order history
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-orders">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="limit">Maximum entries, max 1000.</param>
    /// <param name="startTime">Inclusive UTC lower bound.</param>
    /// <param name="endTime">Inclusive UTC upper bound.</param>
    /// <param name="orderIdFrom">From id</param>
    /// <param name="orderIdTo">To id</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoOrder[]>> GetOrderHistoryAsync(
        string symbol,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? orderIdFrom = null,
        string? orderIdTo = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get user trades
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-trade-history">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="limit">Maximum entries, max 1000</param>
    /// <param name="startTime">Inclusive UTC lower bound.</param>
    /// <param name="endTime">Inclusive UTC upper bound.</param>
    /// <param name="tradeIdFrom">From id</param>
    /// <param name="tradeIdTo">To id</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoFill[]>> GetUserTradesAsync(
        string symbol,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? tradeIdFrom = null,
        string? tradeIdTo = null,
        CancellationToken ct = default);
}
