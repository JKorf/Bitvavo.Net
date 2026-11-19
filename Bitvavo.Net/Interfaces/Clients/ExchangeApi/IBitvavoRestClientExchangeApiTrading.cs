using System;
using System.Threading;
using System.Threading.Tasks;
using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// Bitvavo Exchange trading endpoints, placing and managing orders.
    /// </summary>
    public interface IBitvavoRestClientExchangeApiTrading
    {
        /// <summary>
        /// Place an order
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/create-order/" /><br />
        /// Endpoint:<br />
        /// POST /v2/order
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Trading symbol</param>
        /// <param name="side">["<c>side</c>"] Order side</param>
        /// <param name="type">["<c>orderType</c>"] Order type</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order ID</param>
        /// <param name="quantity">["<c>amount</c>"] Order quantity in the base asset</param>
        /// <param name="quoteQuantity">["<c>amountQuote</c>"] Order quantity in the quote asset</param>
        /// <param name="price">["<c>price</c>"] Limit price</param>
        /// <param name="triggerPrice">["<c>triggerAmount</c>"] Trigger price</param>
        /// <param name="triggerType">["<c>triggerType</c>"] Type of order trigger</param>
        /// <param name="triggerReference">["<c>triggerReference</c>"] Price used to evaluate the trigger</param>
        /// <param name="timeInForce">["<c>timeInForce</c>"] Time in force</param>
        /// <param name="postOnly">["<c>postOnly</c>"] Restrict the order to adding liquidity</param>
        /// <param name="selfTradePrevention">["<c>selfTradePrevention</c>"] Self-trade prevention mode</param>
        /// <param name="responseRequired">["<c>responseRequired</c>"] Return full order details. Defaults to true</param>
        /// <param name="cancelGroupId">["<c>codGroupId</c>"] Cancel-on-disconnect group ID</param>
        /// <param name="operatorId">["<c>operatorId</c>"] Operator ID override. Defaults to the ID configured in the REST options</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoOrder>> PlaceOrderAsync(
            string symbol,
            OrderSide side,
            OrderType type,
            string? clientOrderId = null,
            decimal? quantity = null,
            decimal? quoteQuantity = null,
            decimal? price = null,
            decimal? triggerPrice = null,
            TriggerType? triggerType = null,
            TriggerReference? triggerReference = null,
            TimeInForce? timeInForce = null,
            bool? postOnly = null,
            SelfTradePrevention? selfTradePrevention = null,
            bool? responseRequired = null,
            int? cancelGroupId = null,
            long? operatorId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Update an existing limit or trigger order
        /// Supply an order ID or client order ID. Bitvavo uses the client order ID when both are provided.
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/update-order/" /><br />
        /// Endpoint:<br />
        /// PUT /v2/order
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Trading symbol</param>
        /// <param name="orderId">["<c>orderId</c>"] Bitvavo order ID</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order ID</param>
        /// <param name="quantity">["<c>amount</c>"] Order quantity in the base asset</param>
        /// <param name="quoteQuantity">["<c>amountQuote</c>"] Order quantity in the quote asset</param>
        /// <param name="remainingQuantity">["<c>amountRemaining</c>"] Remaining order quantity in the base asset</param>
        /// <param name="price">["<c>price</c>"] Limit price</param>
        /// <param name="triggerPrice">["<c>triggerAmount</c>"] Trigger price</param>
        /// <param name="timeInForce">["<c>timeInForce</c>"] Time in force</param>
        /// <param name="selfTradePrevention">["<c>selfTradePrevention</c>"] Self-trade prevention mode</param>
        /// <param name="postOnly">["<c>postOnly</c>"] Restrict the order to adding liquidity</param>
        /// <param name="responseRequired">["<c>responseRequired</c>"] Return full order details. Defaults to true</param>
        /// <param name="operatorId">["<c>operatorId</c>"] Operator ID override. Defaults to the ID configured in the REST options</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoOrder>> UpdateOrderAsync(
            string symbol,
            string? orderId = null,
            string? clientOrderId = null,
            decimal? quantity = null,
            decimal? quoteQuantity = null,
            decimal? remainingQuantity = null,
            decimal? price = null,
            decimal? triggerPrice = null,
            TimeInForce? timeInForce = null,
            SelfTradePrevention? selfTradePrevention = null,
            bool? postOnly = null,
            bool? responseRequired = null,
            long? operatorId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get details for an existing order
        /// Supply an order ID or client order ID. Bitvavo uses the client order ID when both are provided.
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-order/" /><br />
        /// Endpoint:<br />
        /// GET /v2/order
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Trading symbol</param>
        /// <param name="orderId">["<c>orderId</c>"] Bitvavo order ID</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order ID</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoOrder>> GetOrderAsync(
            string symbol,
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel an open order
        /// Supply an order ID or client order ID. Bitvavo uses the client order ID when both are provided.
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/cancel-order/" /><br />
        /// Endpoint:<br />
        /// DELETE /v2/order
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Trading symbol</param>
        /// <param name="orderId">["<c>orderId</c>"] Bitvavo order ID</param>
        /// <param name="clientOrderId">["<c>clientOrderId</c>"] Client order ID</param>
        /// <param name="operatorId">["<c>operatorId</c>"] Operator ID override. Defaults to the ID configured in the REST options</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoCanceledOrder>> CancelOrderAsync(
            string symbol,
            string? orderId = null,
            string? clientOrderId = null,
            long? operatorId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get open orders
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-open-orders/" /><br />
        /// Endpoint:<br />
        /// GET /v2/ordersOpen
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Trading symbol filter</param>
        /// <param name="baseAsset">["<c>base</c>"] Base asset filter</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoOrder[]>> GetOpenOrdersAsync(string? symbol = null, string? baseAsset = null, CancellationToken ct = default);

        /// <summary>
        /// Get account trade history for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-trade-history/" /><br />
        /// Endpoint:<br />
        /// GET /v2/trades
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Trading symbol</param>
        /// <param name="fromId">["<c>tradeIdFrom</c>"] Starting ID</param>
        /// <param name="toId">["<c>tradeIdTo</c>"] Ending ID</param>
        /// <param name="startTime">["<c>start</c>"] Start of the time range</param>
        /// <param name="endTime">["<c>end</c>"] End of the time range</param>
        /// <param name="limit">["<c>limit</c>"] Maximum results, up to 1000</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoUserTrade[]>> GetUserTradesAsync(
            string symbol,
            string? fromId = null,
            string? toId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get order history for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-orders/" /><br />
        /// Endpoint:<br />
        /// GET /v2/orders
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Trading symbol</param>
        /// <param name="fromId">["<c>orderIdFrom</c>"] Starting ID</param>
        /// <param name="toId">["<c>orderIdTo</c>"] Ending ID</param>
        /// <param name="startTime">["<c>start</c>"] Start of the time range</param>
        /// <param name="endTime">["<c>end</c>"] End of the time range</param>
        /// <param name="limit">["<c>limit</c>"] Maximum results, up to 1000</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoOrder[]>> GetOrderHistoryAsync(
            string symbol,
            string? fromId = null,
            string? toId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Cancel open orders for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/cancel-orders/" /><br />
        /// Endpoint:<br />
        /// DELETE /v2/orders
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Trading symbol. If omitted, cancels open orders across the account</param>
        /// <param name="operatorId">["<c>operatorId</c>"] Operator ID override. Defaults to the ID configured in the REST options</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoCanceledOrder[]>> CancelOrdersAsync(string? symbol = null, long? operatorId = null, CancellationToken ct = default);

        /// <summary>
        /// Atomically cancel open orders on one side of a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/atomic-cancel-orders/" /><br />
        /// Endpoint:<br />
        /// DELETE /v2/atomic/orders
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Trading symbol</param>
        /// <param name="side">["<c>side</c>"] Order side</param>
        /// <param name="operatorId">["<c>operatorId</c>"] Operator ID override. Defaults to the ID configured in the REST options</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoCanceledOrder[]>> CancelOrdersAtomicAsync(
            string symbol,
            OrderSide side,
            long? operatorId = null,
            CancellationToken ct = default);

        /// <summary>
        /// Set the cancellation countdown for an order group
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/cancel-orders-after/" /><br />
        /// Endpoint:<br />
        /// POST /v2/cancelOrdersAfter
        /// </para>
        /// </summary>
        /// <param name="cancelGroupId">["<c>codGroupId</c>"] Cancel-on-disconnect group ID</param>
        /// <param name="expiryAfterSeconds">["<c>expiryAfterSeconds</c>"] Countdown in seconds, from 10 to 300</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoCancelOrdersAfter>> CancelOrdersAfterAsync(int cancelGroupId, int expiryAfterSeconds, CancellationToken ct = default);
    }
}
