using Bitvavo.Net.Enums;
using Bitvavo.Net.Interfaces.Clients.SpotApi;
using Bitvavo.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Clients.SpotApi;

/// <inheritdoc cref="IBitvavoRestClientSpotApiTrading" />
internal sealed class BitvavoRestClientSpotApiTrading : IBitvavoRestClientSpotApiTrading
{
    private static readonly RequestDefinitionCache _definitions = new();
    private readonly BitvavoRestClientSpotApi _baseClient;

    internal BitvavoRestClientSpotApiTrading(BitvavoRestClientSpotApi baseClient)
    {
        _baseClient = baseClient;
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoOrder>> PlaceOrderAsync(
        string market,
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
        CancellationToken ct = default)
    {
        var body = new Parameters(BitvavoExchange._parameterSerializationSettings);
        body.Add("market", market);
        body.Add("side", side);
        body.Add("orderType", orderType);
        body.Add("operatorId", operatorId);
        body.Add("amount", amount);
        body.Add("amountQuote", amountQuote);
        body.Add("price", price);
        body.Add("triggerAmount", triggerAmount);
        body.Add("triggerType", triggerType);
        body.Add("triggerReference", triggerReference);
        body.Add("timeInForce", timeInForce);
        body.Add("postOnly", postOnly);
        body.Add("selfTradePrevention", selfTradePrevention);
        body.Add("responseRequired", responseRequired);
        body.Add("clientOrderId", clientOrderId);
        body.Add("codGroupId", codGroupId);

        var def = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "v2/order", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: true);
        return _baseClient.SendAsync<BitvavoOrder>(def, queryParameters: null, bodyParameters: body, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoOrder>> UpdateOrderAsync(
        string market,
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
        CancellationToken ct = default)
    {
        var body = new Parameters(BitvavoExchange._parameterSerializationSettings);
        body.Add("market", market);
        body.Add("operatorId", operatorId);
        body.Add("orderId", orderId);
        body.Add("clientOrderId", clientOrderId);
        body.Add("amount", amount);
        body.Add("amountQuote", amountQuote);
        body.Add("price", price);
        body.Add("triggerAmount", triggerAmount);
        body.Add("timeInForce", timeInForce);
        body.Add("selfTradePrevention", selfTradePrevention);
        body.Add("postOnly", postOnly);
        body.Add("responseRequired", responseRequired);

        var def = _definitions.GetOrCreate(HttpMethod.Put, _baseClient.BaseAddress, "v2/order", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: true);
        return _baseClient.SendAsync<BitvavoOrder>(def, queryParameters: null, bodyParameters: body, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoOrder>> GetOrderAsync(string market, string? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("market", market);
        parameters.Add("orderId", orderId);
        parameters.Add("clientOrderId", clientOrderId);

        var def = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/order", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: true);
        return _baseClient.SendAsync<BitvavoOrder>(def, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoOrderId>> CancelOrderAsync(string market, long operatorId, string? orderId = null, string? clientOrderId = null, CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("market", market);
        parameters.Add("operatorId", operatorId);
        parameters.Add("orderId", orderId);
        parameters.Add("clientOrderId", clientOrderId);

        var def = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "v2/order", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: true, parameterPosition: HttpMethodParameterPosition.InUri);
        return _baseClient.SendAsync<BitvavoOrderId>(def, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoOrderId[]>> CancelOrdersAsync(long operatorId, string? market = null, CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("operatorId", operatorId);
        parameters.Add("market", market);

        var weight = market is null ? 100 : 25;
        var def = _definitions.GetOrCreate(HttpMethod.Delete, _baseClient.BaseAddress, "v2/orders", BitvavoExchange.RateLimiter.Bitvavo, weight: weight, authenticated: true);
        return _baseClient.SendAsync<BitvavoOrderId[]>(def, parameters, ct, weight);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoOrder[]>> GetOpenOrdersAsync(string? market = null, string? baseAsset = null, CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("market", market);
        parameters.Add("base", baseAsset);

        var weight = market is null ? 100 : 5;
        var def = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/ordersOpen", BitvavoExchange.RateLimiter.Bitvavo, weight: weight, authenticated: true);
        return _baseClient.SendAsync<BitvavoOrder[]>(def, parameters, ct, weight);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoOrder[]>> GetOrderHistoryAsync(
        string market,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? orderIdFrom = null,
        string? orderIdTo = null,
        CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("market", market);
        parameters.Add("limit", limit);
        parameters.Add("start", startTime);
        parameters.Add("end", endTime);
        parameters.Add("orderIdFrom", orderIdFrom);
        parameters.Add("orderIdTo", orderIdTo);

        var def = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/orders", BitvavoExchange.RateLimiter.Bitvavo, weight: 5, authenticated: true);
        return _baseClient.SendAsync<BitvavoOrder[]>(def, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoFill[]>> GetUserTradesAsync(
        string market,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? tradeIdFrom = null,
        string? tradeIdTo = null,
        CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("market", market);
        parameters.Add("limit", limit);
        parameters.Add("start", startTime);
        parameters.Add("end", endTime);
        parameters.Add("tradeIdFrom", tradeIdFrom);
        parameters.Add("tradeIdTo", tradeIdTo);

        var def = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/trades", BitvavoExchange.RateLimiter.Bitvavo, weight: 5, authenticated: true);
        return _baseClient.SendAsync<BitvavoFill[]>(def, parameters, ct);
    }
}
