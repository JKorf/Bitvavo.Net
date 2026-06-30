using Bitvavo.Net.Interfaces.Clients.SpotApi;
using Bitvavo.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Clients.SpotApi;

/// <inheritdoc cref="IBitvavoRestClientSpotApiFunding" />
internal sealed class BitvavoRestClientSpotApiFunding : IBitvavoRestClientSpotApiFunding
{
    private static readonly RequestDefinitionCache _definitions = new();
    private readonly BitvavoRestClientSpotApi _baseClient;

    internal BitvavoRestClientSpotApiFunding(BitvavoRestClientSpotApi baseClient)
    {
        _baseClient = baseClient;
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
    public Task<HttpResult<BitvavoWithdrawalResult>> WithdrawAsync(BitvavoWithdrawRequest request, CancellationToken ct = default)
    {
        var body = new Parameters(BitvavoExchange._parameterSerializationSettings);
        body.Add("symbol", request.Symbol);
        body.Add("amount", request.Amount);
        body.Add("address", request.Address);
        body.Add("paymentId", request.PaymentId);
        body.Add("addWithdrawalFee", request.AddWithdrawalFee);
        body.Add("internal", request.Internal);

        var def = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "v2/withdrawal", BitvavoExchange.RateLimiter.Bitvavo, weight: 1, authenticated: true);
        return _baseClient.SendAsync<BitvavoWithdrawalResult>(def, queryParameters: null, bodyParameters: body, ct);
    }
}
