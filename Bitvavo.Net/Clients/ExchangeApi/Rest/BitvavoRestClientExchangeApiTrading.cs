using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BitvavoRestClientExchangeApiTrading : IBitvavoRestClientExchangeApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly BitvavoRestClientExchangeApi _baseClient;
        private readonly ILogger _logger;
        private readonly long _defaultOperatorId;

        internal BitvavoRestClientExchangeApiTrading(ILogger logger, BitvavoRestClientExchangeApi baseClient, long defaultOperatorId)
        {
            _baseClient = baseClient;
            _logger = logger;
            _defaultOperatorId = defaultOperatorId;
        }

        #region Place Order

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoOrder>> PlaceOrderAsync(
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

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/order", BitvavoExchange.RateLimiter.Bitvavo, 1, true);
            return await _baseClient.SendAsync<BitvavoOrder>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Update Order

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoOrder>> UpdateOrderAsync(
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

            var request = _definitions.GetOrCreate(HttpMethod.Put, _baseClient.BaseAddress, "/v2/order", BitvavoExchange.RateLimiter.Bitvavo, 1, true);
            return await _baseClient.SendAsync<BitvavoOrder>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoOrder>> GetOrderAsync(
            string symbol,
            string? orderId = null,
            string? clientOrderId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("orderId", orderId);
            parameters.Add("clientOrderId", clientOrderId);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/order", BitvavoExchange.RateLimiter.Bitvavo, 1, true);
            return await _baseClient.SendAsync<BitvavoOrder>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Order

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoCanceledOrder>> CancelOrderAsync(
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

            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/v2/order", BitvavoExchange.RateLimiter.Bitvavo, 1, true, parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync<BitvavoCanceledOrder>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Open Orders

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoOrder[]>> GetOpenOrdersAsync(string? symbol = null, string? baseAsset = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("base", baseAsset);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/ordersOpen", BitvavoExchange.RateLimiter.Bitvavo, 100, true);
            return await _baseClient.SendAsync<BitvavoOrder[]>(request, parameters, ct, weight: symbol == null ? 100 : 5).ConfigureAwait(false);
        }

        #endregion

        #region Get User Trades

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoUserTrade[]>> GetUserTradesAsync(
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

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/trades", BitvavoExchange.RateLimiter.Bitvavo, 5, true);
            return await _baseClient.SendAsync<BitvavoUserTrade[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order History

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoOrder[]>> GetOrderHistoryAsync(
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

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/orders", BitvavoExchange.RateLimiter.Bitvavo, 5, true);
            return await _baseClient.SendAsync<BitvavoOrder[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Orders

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoCanceledOrder[]>> CancelOrdersAsync(string? symbol = null, long? operatorId = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("operatorId", operatorId ?? _defaultOperatorId);

            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/v2/orders", BitvavoExchange.RateLimiter.Bitvavo, 100, true, parameterPosition: HttpMethodParameterPosition.InUri);
            return await _baseClient.SendAsync<BitvavoCanceledOrder[]>(request, parameters, ct, weight: symbol == null ? 100 : 25).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Orders Atomic

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoCanceledOrder[]>> CancelOrdersAtomicAsync(
            string symbol,
            OrderSide side,
            long? operatorId = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("side", side);
            parameters.Add("operatorId", operatorId ?? _defaultOperatorId);

            var request = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "/v2/atomic/orders", BitvavoExchange.RateLimiter.Bitvavo, 100, true);
            return await _baseClient.SendAsync<BitvavoCanceledOrder[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Cancel Orders After

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoCancelOrdersAfter>> CancelOrdersAfterAsync(int cancelGroupId, int expiryAfterSeconds, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("codGroupId", cancelGroupId);
            parameters.Add("expiryAfterSeconds", expiryAfterSeconds);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/cancelOrdersAfter", BitvavoExchange.RateLimiter.Bitvavo, 5, true);
            return await _baseClient.SendAsync<BitvavoCancelOrdersAfter>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
