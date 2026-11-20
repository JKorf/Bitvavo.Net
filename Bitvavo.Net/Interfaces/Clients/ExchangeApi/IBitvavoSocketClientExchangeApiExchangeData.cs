using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using System;
using System.Collections.Generic;
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
        /// Get the current Bitvavo server time
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-server-time/" /><br />
        /// Endpoint:<br />
        /// getTime
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default);

        /// <summary>
        /// Get trading rules and status for a symbol
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-markets/" /><br />
        /// Endpoint:<br />
        /// getMarkets
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Trading symbol</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoSymbol>> GetSymbolAsync(string symbol, CancellationToken ct = default);

        /// <summary>
        /// Get trading rules and status for all symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-markets/" /><br />
        /// Endpoint:<br />
        /// getMarkets
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoSymbol[]>> GetSymbolsAsync(CancellationToken ct = default);

        /// <summary>
        /// Get deposit and withdrawal information for an asset
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-asset-data/" /><br />
        /// Endpoint:<br />
        /// getAssets
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>symbol</c>"] Asset symbol</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoAsset>> GetAssetAsync(string asset, CancellationToken ct = default);

        /// <summary>
        /// Get deposit and withdrawal information for all assets
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-asset-data/" /><br />
        /// Endpoint:<br />
        /// getAssets
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoAsset[]>> GetAssetsAsync(CancellationToken ct = default);

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

        /// <summary>
        /// Receive changes to the best bid, best ask, and latest trade price. Only changed fields are sent in the update
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/ticker-subscription/" /><br />
        /// Channel:<br />
        /// ticker
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>markets</c>"] Symbol</param>
        /// <param name="onMessage">Update handler</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(
            string symbol,
            Action<DataEvent<BitvavoTickerUpdate>> onMessage,
            CancellationToken ct = default);

        /// <summary>
        /// Receive changes to the best bid, best ask, and latest trade price. Only changed fields are sent in the update
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/ticker-subscription/" /><br />
        /// Channel:<br />
        /// ticker
        /// </para>
        /// </summary>
        /// <param name="symbols">["<c>markets</c>"] Symbols</param>
        /// <param name="onMessage">Update handler</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(
            IEnumerable<string> symbols,
            Action<DataEvent<BitvavoTickerUpdate>> onMessage,
            CancellationToken ct = default);

        /// <summary>
        /// Receive 24-hour ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/ticker-24-hour-subscription/" /><br />
        /// Channel:<br />
        /// ticker24h
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>markets</c>"] Symbol</param>
        /// <param name="onMessage">Update handler</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTicker24hUpdatesAsync(
            string symbol,
            Action<DataEvent<BitvavoTicker>> onMessage,
            CancellationToken ct = default);

        /// <summary>
        /// Receive 24-hour ticker updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/ticker-24-hour-subscription/" /><br />
        /// Channel:<br />
        /// ticker24h
        /// </para>
        /// </summary>
        /// <param name="symbols">["<c>markets</c>"] Symbols</param>
        /// <param name="onMessage">Update handler</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTicker24hUpdatesAsync(
            IEnumerable<string> symbols,
            Action<DataEvent<BitvavoTicker>> onMessage,
            CancellationToken ct = default);

        /// <summary>
        /// Receive 24-hour ticker updates in batches, with one callback per message containing updates for the subscribed symbols
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/ticker-24-hour-subscription/" /><br />
        /// Channel:<br />
        /// ticker24h
        /// </para>
        /// </summary>
        /// <param name="symbols">["<c>markets</c>"] Symbols</param>
        /// <param name="onMessage">Update handler</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTicker24hBatchUpdatesAsync(
            IEnumerable<string> symbols,
            Action<DataEvent<BitvavoTicker[]>> onMessage,
            CancellationToken ct = default);

        /// <summary>
        /// Receive candlestick updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/candles-subscription/" /><br />
        /// Channel:<br />
        /// candles
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>markets</c>"] Symbol</param>
        /// <param name="interval">["<c>interval</c>"] Candlestick interval from one minute to one day</param>
        /// <param name="onMessage">Update handler</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(
            string symbol,
            KlineInterval interval,
            Action<DataEvent<BitvavoKlineUpdate>> onMessage,
            CancellationToken ct = default);

        /// <summary>
        /// Receive candlestick updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/candles-subscription/" /><br />
        /// Channel:<br />
        /// candles
        /// </para>
        /// </summary>
        /// <param name="symbols">["<c>markets</c>"] Symbols</param>
        /// <param name="interval">["<c>interval</c>"] Candlestick interval from one minute to one day</param>
        /// <param name="onMessage">Update handler</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(
            IEnumerable<string> symbols,
            KlineInterval interval,
            Action<DataEvent<BitvavoKlineUpdate>> onMessage,
            CancellationToken ct = default);

        /// <summary>
        /// Receive public trade updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/trades-subscription/" /><br />
        /// Channel:<br />
        /// trades
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>markets</c>"] Symbol</param>
        /// <param name="onMessage">Update handler</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
            string symbol,
            Action<DataEvent<BitvavoTradeUpdate>> onMessage,
            CancellationToken ct = default);

        /// <summary>
        /// Receive public trade updates
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/trades-subscription/" /><br />
        /// Channel:<br />
        /// trades
        /// </para>
        /// </summary>
        /// <param name="symbols">["<c>markets</c>"] Symbols</param>
        /// <param name="onMessage">Update handler</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
            IEnumerable<string> symbols,
            Action<DataEvent<BitvavoTradeUpdate>> onMessage,
            CancellationToken ct = default);

        /// <summary>
        /// Receive incremental order book updates. A level with zero quantity is removed
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/book-subscription/" /><br />
        /// Channel:<br />
        /// book
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>markets</c>"] Symbol</param>
        /// <param name="onMessage">Update handler</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
            string symbol,
            Action<DataEvent<BitvavoOrderBook>> onMessage,
            CancellationToken ct = default);

        /// <summary>
        /// Receive incremental order book updates. A level with zero quantity is removed
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/book-subscription/" /><br />
        /// Channel:<br />
        /// book
        /// </para>
        /// </summary>
        /// <param name="symbols">["<c>markets</c>"] Symbols</param>
        /// <param name="onMessage">Update handler</param>
        /// <param name="ct">Cancellation token</param>
        Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
            IEnumerable<string> symbols,
            Action<DataEvent<BitvavoOrderBook>> onMessage,
            CancellationToken ct = default);
    }
}
