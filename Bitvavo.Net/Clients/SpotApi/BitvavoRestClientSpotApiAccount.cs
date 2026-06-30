using Bitvavo.Net.Interfaces.Clients.SpotApi;
using Bitvavo.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Clients.SpotApi;

/// <inheritdoc cref="IBitvavoRestClientSpotApiAccount" />
internal sealed class BitvavoRestClientSpotApiAccount : IBitvavoRestClientSpotApiAccount
{
    private static readonly RequestDefinitionCache _definitions = new();
    private readonly BitvavoRestClientSpotApi _baseClient;

    internal BitvavoRestClientSpotApiAccount(BitvavoRestClientSpotApi baseClient)
    {
        _baseClient = baseClient;
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoAccountInfo>> GetAccountInfoAsync(CancellationToken ct = default)
    {
        // weight assumed â€” Bitvavo docs page did not state it; conservative default
        var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/account", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: true);
        return _baseClient.SendAsync<BitvavoAccountInfo>(request, parameters: null, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<IEnumerable<BitvavoBalance>>> GetBalancesAsync(string? symbol = null, CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("symbol", symbol);

        var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/balance", BitvavoExchange.RateLimiter.Bitvavo, weight: 5, authenticated: true);
        return _baseClient.SendAsync<IEnumerable<BitvavoBalance>>(request, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoMarketFee>> GetTradingFeesAsync(string? market = null, CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("market", market);

        var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/account/fees", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: true);
        return _baseClient.SendAsync<BitvavoMarketFee>(request, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoCancelOrdersAfter>> ResetCancelOnDisconnectAsync(
        int codGroupId, int expiryAfterSeconds, CancellationToken ct = default)
    {
        var body = new Parameters(BitvavoExchange._parameterSerializationSettings);
        body.Add("codGroupId", codGroupId);
        body.Add("expiryAfterSeconds", expiryAfterSeconds);
        // Intentionally NOT gated â€” the CoD heartbeat must never be client-side rate-limited; its
        // ~30 weight/min is absorbed by RateLimitGate's ClientSafetyMargin. (P1-bis mini-council 2026-05-20.)
        var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "v2/cancelOrdersAfter", true);
        return _baseClient.SendAsync<BitvavoCancelOrdersAfter>(request, queryParameters: null, bodyParameters: body, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoTransactionHistory>> GetTransactionHistoryAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? page = null,
        int? maxItems = null,
        string? type = null,
        CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("fromDate", fromDate);
        parameters.Add("toDate", toDate);
        parameters.Add("page", page);
        parameters.Add("maxItems", maxItems);
        parameters.Add("type", type);

        // weight assumed â€” Bitvavo docs page did not state it; conservative default
        var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/account/history", BitvavoExchange.RateLimiter.Bitvavo, weight: 5, authenticated: true);
        return _baseClient.SendAsync<BitvavoTransactionHistory>(request, parameters, ct);
    }
}
