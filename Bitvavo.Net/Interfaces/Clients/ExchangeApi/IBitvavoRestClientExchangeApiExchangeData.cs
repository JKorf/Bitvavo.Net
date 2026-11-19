using System;
using System.Threading;
using System.Threading.Tasks;
using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// Bitvavo Exchange exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IBitvavoRestClientExchangeApiExchangeData
    {
        /// <summary>
        /// Get the current Bitvavo server time
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-server-time/" /><br />
        /// Endpoint:<br />
        /// GET /v2/time
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get trading rules and status for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-markets/" /><br />
        /// Endpoint:<br />
        /// GET /v2/markets
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Trading symbol</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoSymbol>> GetSymbolAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get trading rules and status for all symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-markets/" /><br />
        /// Endpoint:<br />
        /// GET /v2/markets
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoSymbol[]>> GetSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get deposit and withdrawal information for an asset
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-asset-data/" /><br />
        /// Endpoint:<br />
        /// GET /v2/assets
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>symbol</c>"] Asset symbol</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoAsset>> GetAssetAsync(string asset, CancellationToken ct = default);

        /// <summary>
        /// Get deposit and withdrawal information for all assets
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-asset-data/" /><br />
        /// Endpoint:<br />
        /// GET /v2/assets
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoAsset[]>> GetAssetsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get trades for a period of up to 24 hours
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-trades/" /><br />
        /// Endpoint:<br />
        /// GET /v2/{market}/trades
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Filter by symbol, for example `ETH-USDC`</param>
        /// <param name="fromId">["<c>tradeIdFrom</c>"] Filter from id</param>
        /// <param name="toId">["<c>tradeIdTo</c>"] Filter to id</param>
        /// <param name="startTime">["<c>start</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 1000</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoTrade[]>> GetTradesAsync(
            string symbol,
            string? fromId = null,
            string? toId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get an order book snapshot
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-order-book/" /><br />
        /// Endpoint:<br />
        /// GET /v2/{market}/book
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol, for example BTC-EUR</param>
        /// <param name="depth">["<c>depth</c>"] Number of levels to return per side, up to 1000</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default);

        /// <summary>
        /// Get a MiCA order book report
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-order-book-report/" /><br />
        /// Endpoint:<br />
        /// GET /v2/report/{market}/book
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol, for example BTC-EUR</param>
        /// <param name="depth">["<c>depth</c>"] Number of levels to return per side, up to 1000</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoOrderBookReport>> GetOrderBookReportAsync(string symbol, int? depth = null, CancellationToken ct = default);

        /// <summary>
        /// Get MiCA trade reports for a period of up to 24 hours
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-trades-report/" /><br />
        /// Endpoint:<br />
        /// GET /v2/report/{market}/trades
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol, for example BTC-EUR</param>
        /// <param name="fromId">["<c>tradeIdFrom</c>"] Starting trade ID</param>
        /// <param name="toId">["<c>tradeIdTo</c>"] Ending trade ID</param>
        /// <param name="startTime">["<c>start</c>"] Start time</param>
        /// <param name="endTime">["<c>end</c>"] End time, up to 24 hours after the start time</param>
        /// <param name="limit">["<c>limit</c>"] Maximum number of trades to return, up to 1000</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoTradeReport[]>> GetTradesReportAsync(
            string symbol,
            string? fromId = null,
            string? toId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get candlesticks for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-candlestick-data/" /><br />
        /// Endpoint:<br />
        /// GET /v2/{market}/candles
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol, for example BTC-EUR</param>
        /// <param name="interval">["<c>interval</c>"] Candlestick interval</param>
        /// <param name="startTime">["<c>start</c>"] Start time aligned with the selected interval</param>
        /// <param name="endTime">["<c>end</c>"] End time aligned with the selected interval</param>
        /// <param name="limit">["<c>limit</c>"] Maximum number of candlesticks to return, up to 1440</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoKline[]>> GetKlinesAsync(
            string symbol,
            KlineInterval interval,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get the last trade price for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-ticker-prices/" /><br />
        /// Endpoint:<br />
        /// GET /v2/ticker/price
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol, for example BTC-EUR</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoTickerPrice>> GetTickerPriceAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get the last trade prices for all symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-ticker-prices/" /><br />
        /// Endpoint:<br />
        /// GET /v2/ticker/price
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoTickerPrice[]>> GetTickerPricesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get the best bid and ask for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-ticker-book/" /><br />
        /// Endpoint:<br />
        /// GET /v2/ticker/book
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol, for example BTC-EUR</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get the best bids and asks for all symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-ticker-book/" /><br />
        /// Endpoint:<br />
        /// GET /v2/ticker/book
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoBookTicker[]>> GetBookTickersAsync(CancellationToken ct = default);

        /// <summary>
        /// Get 24-hour ticker data for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-ticker-data-24-h/" /><br />
        /// Endpoint:<br />
        /// GET /v2/ticker/24h
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol, for example BTC-EUR</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoTicker>> GetTickerAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get 24-hour ticker data for all symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-ticker-data-24-h/" /><br />
        /// Endpoint:<br />
        /// GET /v2/ticker/24h
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoTicker[]>> GetTickersAsync(CancellationToken ct = default);
    }
}
