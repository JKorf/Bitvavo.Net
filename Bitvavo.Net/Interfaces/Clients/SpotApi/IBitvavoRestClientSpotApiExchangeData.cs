using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Interfaces.Clients.SpotApi;

/// <summary>
/// Public-data REST endpoints
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
    Task<HttpResult<BitvavoSymbol[]>> GetSymbolsAsync(CancellationToken ct = default);

    /// <summary>
    /// Get historical candles (klines) for a given market and interval.
    /// <para>
    /// <a href="https://docs.bitvavo.com/docs/rest-api/get-candles">Bitvavo API docs</a>
    /// </para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="interval">Candle interval (1m, 5m, 1h, 1d, etc.).</param>
    /// <param name="limit">Maximum candles to return, max 1440</param>
    /// <param name="startTime">Inclusive UTC lower bound.</param>
    /// <param name="endTime">Inclusive UTC upper bound.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoKline[]>> GetKlinesAsync(
        string symbol,
        KlineInterval interval,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get the current server time
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-server-time">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoServerTime>> GetServerTimeAsync(CancellationToken ct = default);

    /// <summary>
    /// Get supported assets
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-asset-data">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="asset">Asset</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoAsset[]>> GetAssetsAsync(string? asset = null, CancellationToken ct = default);

    /// <summary>
    /// Get the most-recent traded price for one or all symbols.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-ticker-prices">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoTickerPrice[]>> GetPricesAsync(string? symbol = null, CancellationToken ct = default);

    /// <summary>
    /// Get the best book price for one or all symbols.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-ticker-book">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoTickerBook[]>> GetBookTickerAsync(string? symbol = null, CancellationToken ct = default);

    /// <summary>
    /// Get the 24-hour rolling price info
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-candlestick-data-24-h">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoTicker24h[]>> GetTickersAsync(string? symbol = null, CancellationToken ct = default);

    /// <summary>
    /// Get the order-book snapshot
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-order-book">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="depth">Maximum number of entries per side to return, max 1000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default);

    /// <summary>
    /// Get trade history
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-trades">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="limit">Maximum number of results, max 1000</param>
    /// <param name="startTime">Inclusive UTC lower bound. Bitvavo limits the window to 24 hours.</param>
    /// <param name="endTime">Inclusive UTC upper bound.</param>
    /// <param name="tradeIdFrom">From trade id</param>
    /// <param name="tradeIdTo">To trade id</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoPublicTrade[]>> GetTradeHistoryAsync(
        string symbol,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? tradeIdFrom = null,
        string? tradeIdTo = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get the MiCA trade report for a symbol and time window (max 24-hour range).
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-trades-report">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="limit">Maximum trades to return, max 1000.</param>
    /// <param name="startTime">Inclusive UTC lower bound. The window may span at most 24 hours.</param>
    /// <param name="endTime">Inclusive UTC upper bound â€” at most 24 hours after <paramref name="startTime"/>.</param>
    /// <param name="tradeIdFrom">From trade id</param>
    /// <param name="tradeIdTo">To trade id</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoTradesReport[]>> GetTradesReportAsync(
        string symbol,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        string? tradeIdFrom = null,
        string? tradeIdTo = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get the MiCA order-book report
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-order-book-report">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="depth">Number of bid / ask price levels to return, max 1000.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoBookReport>> GetBookReportAsync(string symbol, int? depth = null, CancellationToken ct = default);
}
