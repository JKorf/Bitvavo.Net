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

        var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/account/history", BitvavoExchange.RateLimiter.Bitvavo, weight: 5, authenticated: true);
        return _baseClient.SendAsync<BitvavoTransactionHistory>(request, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoDepositAddress>> GetDepositAddressAsync(string symbol, CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings) { { "symbol", symbol } };
        var def = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/deposit", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: true);
        return _baseClient.SendAsync<BitvavoDepositAddress>(def, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<IEnumerable<BitvavoDepositHistoryEntry>>> GetDepositHistoryAsync(
        string? symbol = null,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("symbol", symbol);
        parameters.Add("limit", limit);
        parameters.Add("start", startTime);
        parameters.Add("end", endTime);

        var def = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/depositHistory", BitvavoExchange.RateLimiter.Bitvavo, weight: 5, authenticated: true);
        return _baseClient.SendAsync<IEnumerable<BitvavoDepositHistoryEntry>>(def, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<IEnumerable<BitvavoWithdrawalHistoryEntry>>> GetWithdrawalHistoryAsync(
        string? symbol = null,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        CancellationToken ct = default)
    {
        var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
        parameters.Add("symbol", symbol);
        parameters.Add("limit", limit);
        parameters.Add("start", startTime);
        parameters.Add("end", endTime);

        var def = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "v2/withdrawalHistory", BitvavoExchange.RateLimiter.Bitvavo, weight: 5, authenticated: true);
        return _baseClient.SendAsync<IEnumerable<BitvavoWithdrawalHistoryEntry>>(def, parameters, ct);
    }

    /// <inheritdoc />
    public Task<HttpResult<BitvavoWithdrawalResult>> WithdrawAsync(
        string symbol,
        decimal amount,
        string address,
        string? paymentId = null,
        bool? addWithdrawalFee = null,
        bool? internalTransfer = null,
        CancellationToken ct = default)
    {
        var body = new Parameters(BitvavoExchange._parameterSerializationSettings);
        body.Add("symbol", symbol);
        body.Add("amount", amount);
        body.Add("address", address);
        body.Add("paymentId", paymentId);
        body.Add("addWithdrawalFee", addWithdrawalFee);
        body.Add("internal", internalTransfer);

        var def = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "v2/withdrawal", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: true);
        return _baseClient.SendAsync<BitvavoWithdrawalResult>(def, queryParameters: null, bodyParameters: body, ct);
    }
}
