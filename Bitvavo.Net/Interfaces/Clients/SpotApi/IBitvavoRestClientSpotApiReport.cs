using Bitvavo.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Interfaces.Clients.SpotApi;

/// <summary>
/// Bitvavo MiCA regulatory-reporting REST endpoints â€” trade and order-book reports
/// formatted to the EU Markets-in-Crypto-Assets reporting schema. These endpoints are
/// public, but authenticating the request grants a higher rate limit, so this SDK signs
/// them like every other endpoint.
/// </summary>
public interface IBitvavoRestClientSpotApiReport
{
    /// <summary>
    /// Get the MiCA trade report for a single market and time window (max 24-hour range).
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-trades-report">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="limit">Maximum trades to return, max 1000.</param>
    /// <param name="startTime">Inclusive UTC lower bound. The window may span at most 24 hours.</param>
    /// <param name="endTime">Inclusive UTC upper bound â€” at most 24 hours after <paramref name="startTime"/>.</param>
    /// <param name="tradeIdFrom">Pagination cursor â€” only return trades from this trade id onward.</param>
    /// <param name="tradeIdTo">Pagination cursor â€” only return trades up to this trade id.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<IEnumerable<BitvavoTradesReport>>> GetTradesReportAsync(
        string symbol,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? tradeIdFrom = null,
        string? tradeIdTo = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get the MiCA order-book report for a single market.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-order-book-report">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="depth">Number of bid / ask price levels to return, max 1000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoBookReport>> GetBookReportAsync(string symbol, int? depth = null, CancellationToken ct = default);
}
