using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bitvavo.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// Bitvavo WebSocket exchange data endpoints
    /// </summary>
    public interface IBitvavoSocketClientExchangeApiExchangeData
    {
        /// <summary>
        /// Get the order book for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-order-book/" /><br />
        /// Endpoint:<br />
        /// getBook
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol</param>
        /// <param name="depth">["<c>depth</c>"] Maximum number of levels per side, up to 1000</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default);

        /// <summary>
        /// Get recent trades for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-trades/" /><br />
        /// Endpoint:<br />
        /// getTrades
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol</param>
        /// <param name="fromId">["<c>tradeIdFrom</c>"] First trade ID</param>
        /// <param name="toId">["<c>tradeIdTo</c>"] Last trade ID</param>
        /// <param name="startTime">["<c>start</c>"] Start time</param>
        /// <param name="endTime">["<c>end</c>"] End time</param>
        /// <param name="limit">["<c>limit</c>"] Maximum number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoTrade[]>> GetTradesAsync(
            string symbol,
            string? fromId = null,
            string? toId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get the latest trade price for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-ticker-prices/" /><br />
        /// Endpoint:<br />
        /// getTickerPrice
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoTickerPrice>> GetTickerPriceAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get the latest trade prices for all symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-ticker-prices/" /><br />
        /// Endpoint:<br />
        /// getTickerPrice
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoTickerPrice[]>> GetTickerPricesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the best bid and ask for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-ticker-book/" /><br />
        /// Endpoint:<br />
        /// getTickerBook
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get the best bid and ask for all symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-ticker-book/" /><br />
        /// Endpoint:<br />
        /// getTickerBook
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoBookTicker[]>> GetBookTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get candlesticks for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-candlestick-data/" /><br />
        /// Endpoint:<br />
        /// getCandles
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol</param>
        /// <param name="interval">["<c>interval</c>"] Candlestick interval</param>
        /// <param name="startTime">["<c>start</c>"] Start time</param>
        /// <param name="endTime">["<c>end</c>"] End time</param>
        /// <param name="limit">["<c>limit</c>"] Maximum number of results</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoKline[]>> GetKlinesAsync(
            string symbol,
            KlineInterval interval,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get 24-hour ticker data for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-ticker-data-24-h/" /><br />
        /// Endpoint:<br />
        /// getTicker24h
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoTicker>> GetTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get 24-hour ticker data for all symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-ticker-data-24-h/" /><br />
        /// Endpoint:<br />
        /// getTicker24h
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoTicker[]>> GetTickersAsync(CancellationToken ct = default);
    }
}
