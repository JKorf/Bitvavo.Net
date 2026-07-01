using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Interfaces.Clients.SpotApi;

/// <summary>
/// Bitvavo public-data REST endpoints â€” no authentication required. Includes market
/// discovery and historical kline (candle) retrieval.
/// </summary>
public interface IBitvavoRestClientSpotApiExchangeData
{
    /// <summary>
    /// Get all spot markets supported by Bitvavo.
    /// <para>
    /// <a href="https://docs.bitvavo.com/docs/rest-api/get-markets">Bitvavo API docs</a>
    /// </para>
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<IEnumerable<BitvavoSymbol>>> GetSymbolsAsync(CancellationToken ct = default);

    /// <summary>
    /// Get historical candles (klines) for a given market and interval.
    /// <para>
    /// <a href="https://docs.bitvavo.com/docs/rest-api/get-candles">Bitvavo API docs</a>
    /// </para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="interval">Candle interval (1m, 5m, 1h, 1d, etc.).</param>
    /// <param name="limit">Maximum candles to return (1-1440). Defaults to Bitvavo's server default if omitted.</param>
    /// <param name="startTime">Inclusive UTC lower bound â€” fetches candles starting from this time.</param>
    /// <param name="endTime">Inclusive UTC upper bound.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<IEnumerable<BitvavoKline>>> GetKlinesAsync(
        string symbol,
        KlineInterval interval,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get the current server time. Useful for clock-drift detection before signed
    /// requests â€” <c>Bitvavo-Access-Window</c> rejects requests too far from the server's clock.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-server-time">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoServerTime>> GetServerTimeAsync(CancellationToken ct = default);

    /// <summary>
    /// Get all Bitvavo-supported assets, optionally filtered to a single symbol.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-asset-data">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<IEnumerable<BitvavoAsset>>> GetAssetsAsync(string? asset = null, CancellationToken ct = default);

    /// <summary>
    /// Get the most-recent traded price for one or all symbols.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-ticker-prices">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<IEnumerable<BitvavoTickerPrice>>> GetPricesAsync(string? symbol = null, CancellationToken ct = default);

    /// <summary>
    /// Get the top-of-book quote (best bid + best ask) for one or all symbols.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-ticker-book">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<IEnumerable<BitvavoTickerBook>>> GetBookTickerAsync(string? symbol = null, CancellationToken ct = default);

    /// <summary>
    /// Get the 24-hour rolling OHLCV + best-quote stats for one or all symbols.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-candlestick-data-24-h">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<IEnumerable<BitvavoTicker24h>>> GetTickersAsync(string? symbol = null, CancellationToken ct = default);

    /// <summary>
    /// Get the order-book snapshot for a single symbol.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-order-book">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="depth">Maximum entries per side (1â€“1000, default 1000).</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default);

    /// <summary>
    /// Get trade history
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-trades">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="limit">Maximum entries (1â€“1000, default 500).</param>
    /// <param name="startTime">Inclusive UTC lower bound. Bitvavo limits the window to 24 hours.</param>
    /// <param name="endTime">Inclusive UTC upper bound.</param>
    /// <param name="tradeIdFrom">Pagination cursor â€” only return trades with id &gt;= this value.</param>
    /// <param name="tradeIdTo">Pagination cursor â€” only return trades with id &lt;= this value.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<IEnumerable<BitvavoPublicTrade>>> GetTradeHistoryAsync(
        string symbol,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? tradeIdFrom = null,
        string? tradeIdTo = null,
        CancellationToken ct = default);
}
