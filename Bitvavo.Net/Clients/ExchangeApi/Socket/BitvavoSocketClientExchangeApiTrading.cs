using Bitvavo.Net.Enums;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BitvavoSocketClientExchangeApiTrading : IBitvavoSocketClientExchangeApiTrading
    {
        private readonly BitvavoSocketClientExchangeApi _baseClient;
        private readonly long _defaultOperatorId;

        internal BitvavoSocketClientExchangeApiTrading(BitvavoSocketClientExchangeApi baseClient, long defaultOperatorId)
        {
            _baseClient = baseClient;
            _defaultOperatorId = defaultOperatorId;
        }

        #region Place Order

        /// <inheritdoc />
        public Task<QueryResult<BitvavoOrder>> PlaceOrderAsync(
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
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("side", side);
            parameters.Add("orderType", type);
            parameters.Add("clientOrderId", clientOrderId);
            parameters.Add("amount", quantity);
            parameters.Add("amountQuote", quoteQuantity);
            parameters.Add("price", price);
            parameters.Add("triggerAmount", triggerPrice);
            parameters.Add("triggerType", triggerType);
            parameters.Add("triggerReference", triggerReference);
            parameters.Add("timeInForce", timeInForce);
            parameters.Add("postOnly", postOnly);
            parameters.Add("selfTradePrevention", selfTradePrevention);
            parameters.Add("responseRequired", responseRequired);
            parameters.Add("codGroupId", cancelGroupId);
            parameters.Add("operatorId", operatorId ?? _defaultOperatorId);

            return _baseClient.SendPrivateAsync<BitvavoOrder>("privateCreateOrder", parameters, ct);
        }

        #endregion

        #region Update Order

        /// <inheritdoc />
        public Task<QueryResult<BitvavoOrder>> UpdateOrderAsync(
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
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("orderId", orderId);
            parameters.Add("clientOrderId", clientOrderId);
            parameters.Add("amount", quantity);
            parameters.Add("amountQuote", quoteQuantity);
            parameters.Add("amountRemaining", remainingQuantity);
            parameters.Add("price", price);
            parameters.Add("triggerAmount", triggerPrice);
            parameters.Add("timeInForce", timeInForce);
            parameters.Add("selfTradePrevention", selfTradePrevention);
            parameters.Add("postOnly", postOnly);
            parameters.Add("responseRequired", responseRequired);
            parameters.Add("operatorId", operatorId ?? _defaultOperatorId);

            return _baseClient.SendPrivateAsync<BitvavoOrder>("privateUpdateOrder", parameters, ct);
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public Task<QueryResult<BitvavoOrder>> GetOrderAsync(
            string symbol,
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("orderId", orderId);
            parameters.Add("clientOrderId", clientOrderId);

            return _baseClient.SendPrivateAsync<BitvavoOrder>("privateGetOrder", parameters, ct);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public Task<QueryResult<BitvavoCanceledOrder>> CancelOrderAsync(
            string symbol,
            string? orderId = null,
            string? clientOrderId = null,
            long? operatorId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("orderId", orderId);
            parameters.Add("clientOrderId", clientOrderId);
            parameters.Add("operatorId", operatorId ?? _defaultOperatorId);

            return _baseClient.SendPrivateAsync<BitvavoCanceledOrder>("privateCancelOrder", parameters, ct);
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public Task<QueryResult<BitvavoOrder[]>> GetOpenOrdersAsync(string? symbol = null, string? baseAsset = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("base", baseAsset);

            return _baseClient.SendPrivateAsync<BitvavoOrder[]>("privateGetOrdersOpen", parameters, ct, weight: symbol == null ? 100 : 5);
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public Task<QueryResult<BitvavoUserTrade[]>> GetUserTradesAsync(
            string symbol,
            string? fromId = null,
            string? toId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("tradeIdFrom", fromId);
            parameters.Add("tradeIdTo", toId);
            parameters.Add("start", startTime);
            parameters.Add("end", endTime);
            parameters.Add("limit", limit);

            return _baseClient.SendPrivateAsync<BitvavoUserTrade[]>("privateGetTrades", parameters, ct, weight: 5);
        }

        #endregion

        #region Get Order History

        /// <inheritdoc />
        public Task<QueryResult<BitvavoOrder[]>> GetOrderHistoryAsync(
            string symbol,
            string? fromId = null,
            string? toId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("orderIdFrom", fromId);
            parameters.Add("orderIdTo", toId);
            parameters.Add("start", startTime);
            parameters.Add("end", endTime);
            parameters.Add("limit", limit);

            return _baseClient.SendPrivateAsync<BitvavoOrder[]>("privateGetOrders", parameters, ct, weight: 5);
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public Task<QueryResult<BitvavoCanceledOrder[]>> CancelOrdersAsync(string? symbol = null, long? operatorId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("operatorId", operatorId ?? _defaultOperatorId);

            return _baseClient.SendPrivateAsync<BitvavoCanceledOrder[]>("privateCancelOrders", parameters, ct, weight: symbol == null ? 100 : 25);
        }

        #endregion

        #region Cancel Orders Atomic

        /// <inheritdoc />
        public Task<QueryResult<BitvavoCanceledOrder[]>> CancelOrdersAtomicAsync(
            string symbol,
            OrderSide side,
            long? operatorId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("side", side);
            parameters.Add("operatorId", operatorId ?? _defaultOperatorId);

            return _baseClient.SendPrivateAsync<BitvavoCanceledOrder[]>("privateAtomicCancelOrders", parameters, ct, weight: 100);
        }

        #endregion

        #region Cancel Orders After

        /// <inheritdoc />
        public Task<QueryResult<BitvavoCancelOrdersAfter>> CancelOrdersAfterAsync(int cancelGroupId, int expiryAfterSeconds, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("codGroupId", cancelGroupId);
            parameters.Add("expiryAfterSeconds", expiryAfterSeconds);

            return _baseClient.SendPrivateAsync<BitvavoCancelOrdersAfter>("privateCancelOrdersAfter", parameters, ct, weight: 5);
        }

        #endregion
    }
}
