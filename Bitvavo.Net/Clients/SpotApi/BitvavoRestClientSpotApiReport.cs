using Bitvavo.Net.Interfaces.Clients.SpotApi;
using Bitvavo.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Clients.SpotApi;

/// <inheritdoc cref="IBitvavoRestClientSpotApiReport" />
internal sealed class BitvavoRestClientSpotApiReport : IBitvavoRestClientSpotApiReport
{
    private static readonly RequestDefinitionCache _definitions = new();
    private readonly BitvavoRestClientSpotApi _baseClient;

    internal BitvavoRestClientSpotApiReport(BitvavoRestClientSpotApi baseClient)
    {
        _baseClient = baseClient;
    }

    /// <inheritdoc />
    public Task<HttpResult<IEnumerable<BitvavoTradesReport>>> GetTradesReportAsync(
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
        return _baseClient.SendAsync<IEnumerable<BitvavoTradesReport>>(def, parameters, ct);
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
