using Bitvavo.Net.Enums;
using Bitvavo.Net.Interfaces.Clients.SpotApi;
using Bitvavo.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Clients.SpotApi;

/// <inheritdoc />
internal sealed class BitvavoRestClientSpotApiExchangeData : IBitvavoRestClientSpotApiExchangeData
{
    private static readonly RequestDefinitionCache _definitions = new();
    private readonly BitvavoRestClientSpotApi _baseClient;

    internal BitvavoRestClientSpotApiExchangeData(BitvavoRestClientSpotApi baseClient)
    {
        _baseClient = baseClient;
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoSymbol[]>> GetSymbolsAsync(CancellationToken ct = default)
    {
        var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/markets", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: false);
        return _baseClient.SendAsync<BitvavoSymbol[]>(request, parameters: null, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoKline[]>> GetKlinesAsync(
        string market,
        KlineInterval interval,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("interval", interval);
        parameters.Add("limit", limit);
        parameters.Add("start", startTime);
        parameters.Add("end", endTime);

        var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, $"v2/{market}/candles", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: false);
        return _baseClient.SendAsync<BitvavoKline[]>(request, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoServerTime>> GetServerTimeAsync(CancellationToken ct = default)
    {
        var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/time", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: false);
        return _baseClient.SendAsync<BitvavoServerTime>(request, parameters: null, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoAsset[]>> GetAssetsAsync(string? symbol = null, CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("symbol", symbol);

        var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/assets", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: false);
        return _baseClient.SendAsync<BitvavoAsset[]>(request, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoTickerPrice[]>> GetPricesAsync(string? market = null, CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("market", market);

        var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/ticker/price", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: false);
        return _baseClient.SendAsync<BitvavoTickerPrice[]>(request, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoTickerBook[]>> GetBookTickerAsync(string? market = null, CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("market", market);

        var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/ticker/book", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: false);
        return _baseClient.SendAsync<BitvavoTickerBook[]>(request, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoTicker24h[]>> GetTickersAsync(string? market = null, CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("market", market);

        var weight = market is null ? 25 : 1;
        var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/ticker/24h", BitvavoExchange.RateLimiter.Bitvavo, weight: weight, authenticated: false);
        return _baseClient.SendAsync<BitvavoTicker24h[]>(request, parameters, ct, weight: weight);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoOrderBook>> GetOrderBookAsync(string market, int? depth = null, CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("depth", depth);

        var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, $"v2/{market}/book", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: false);
        return _baseClient.SendAsync<BitvavoOrderBook>(request, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoPublicTrade[]>> GetTradeHistoryAsync(
        string market,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? tradeIdFrom = null,
        string? tradeIdTo = null,
        CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("limit", limit);
        parameters.Add("start", startTime);
        parameters.Add("end", endTime);
        parameters.Add("tradeIdFrom", tradeIdFrom);
        parameters.Add("tradeIdTo", tradeIdTo);

        var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, $"v2/{market}/trades", BitvavoExchange.RateLimiter.Bitvavo, weight: 5, authenticated: false);
        return _baseClient.SendAsync<BitvavoPublicTrade[]>(request, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoTradesReport[]>> GetTradesReportAsync(
        string market,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? tradeIdFrom = null,
        string? tradeIdTo = null,
        CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("limit", limit);
        parameters.Add("start", startTime);
        parameters.Add("end", endTime);
        parameters.Add("tradeIdFrom", tradeIdFrom);
        parameters.Add("tradeIdTo", tradeIdTo);

        var def = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, $"v2/report/{market}/trades", BitvavoExchange.RateLimiter.Bitvavo, weight: 5, authenticated: true);
        return _baseClient.SendAsync<BitvavoTradesReport[]>(def, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoBookReport>> GetBookReportAsync(string market, int? depth = null, CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("depth", depth);

        var def = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, $"v2/report/{market}/book", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: true);
        return _baseClient.SendAsync<BitvavoBookReport>(def, parameters, ct);
    }
}
