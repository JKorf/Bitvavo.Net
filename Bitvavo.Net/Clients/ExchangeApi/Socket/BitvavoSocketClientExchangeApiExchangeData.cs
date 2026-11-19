using Bitvavo.Net.Enums;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using Bitvavo.Net.Objects.Internal;
using Bitvavo.Net.Objects.Models;
using Bitvavo.Net.Objects.Sockets.Subscriptions;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BitvavoSocketClientExchangeApiExchangeData : IBitvavoSocketClientExchangeApiExchangeData
    {
        private readonly ILogger _logger;
        private readonly BitvavoSocketClientExchangeApi _baseClient;

        internal BitvavoSocketClientExchangeApiExchangeData(ILogger logger, BitvavoSocketClientExchangeApi baseClient)
        {
            _logger = logger;
            _baseClient = baseClient;
        }

        #region Get Order Book

        /// <inheritdoc />
        public Task<QueryResult<BitvavoOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("depth", depth);

            return _baseClient.SendAsync<BitvavoOrderBook>("getBook", parameters, ct);
        }

        #endregion

        #region Get Trades

        /// <inheritdoc />
        public Task<QueryResult<BitvavoTrade[]>> GetTradesAsync(
            string symbol,
            string? fromId = null,
            string? toId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("tradeIdFrom", fromId);
            parameters.Add("tradeIdTo", toId);
            parameters.Add("start", startTime);
            parameters.Add("end", endTime);
            parameters.Add("limit", limit);

            return _baseClient.SendAsync<BitvavoTrade[]>("getTrades", parameters, ct, weight: 5);
        }

        #endregion

        #region Get Ticker Price

        /// <inheritdoc />
        public Task<QueryResult<BitvavoTickerPrice>> GetTickerPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);

            return _baseClient.SendAsync<BitvavoTickerPrice>("getTickerPrice", parameters, ct);
        }

        #endregion

        #region Get Ticker Prices

        /// <inheritdoc />
        public Task<QueryResult<BitvavoTickerPrice[]>> GetTickerPricesAsync(CancellationToken ct = default)
            => _baseClient.SendAsync<BitvavoTickerPrice[]>("getTickerPrice", null, ct);

        #endregion

        #region Get Book Ticker

        /// <inheritdoc />
        public Task<QueryResult<BitvavoBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);

            return _baseClient.SendAsync<BitvavoBookTicker>("getTickerBook", parameters, ct);
        }

        #endregion

        #region Get Book Tickers

        /// <inheritdoc />
        public Task<QueryResult<BitvavoBookTicker[]>> GetBookTickersAsync(CancellationToken ct = default)
            => _baseClient.SendAsync<BitvavoBookTicker[]>("getTickerBook", null, ct);

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public Task<QueryResult<BitvavoKline[]>> GetKlinesAsync(
            string symbol,
            KlineInterval interval,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("interval", interval);
            parameters.Add("start", startTime);
            parameters.Add("end", endTime);
            parameters.Add("limit", limit);

            return _baseClient.SendAsync<BitvavoKline[]>("getCandles", parameters, ct);
        }

        #endregion

        #region Get Ticker

        /// <inheritdoc />
        public Task<QueryResult<BitvavoTicker>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);

            return _baseClient.SendAsync<BitvavoTicker>("getTicker24h", parameters, ct);
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public Task<QueryResult<BitvavoTicker[]>> GetTickersAsync(CancellationToken ct = default)
            => _baseClient.SendAsync<BitvavoTicker[]>("getTicker24h", null, ct, weight: 25);

        #endregion

        #region Subscribe To Ticker Updates

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(
            string symbol,
            Action<DataEvent<BitvavoTickerUpdate>> onMessage,
            CancellationToken ct = default)
            => SubscribeToTickerUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(
            IEnumerable<string> symbols,
            Action<DataEvent<BitvavoTickerUpdate>> onMessage,
            CancellationToken ct = default)
            => SubscribeToUpdatesAsync("ticker", "ticker", symbols, onMessage, update => update.Symbol, ct: ct);

        #endregion

        #region Subscribe To Ticker24h Updates

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTicker24hUpdatesAsync(
            string symbol,
            Action<DataEvent<BitvavoTicker>> onMessage,
            CancellationToken ct = default)
            => SubscribeToTicker24hUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTicker24hUpdatesAsync(
            IEnumerable<string> symbols,
            Action<DataEvent<BitvavoTicker>> onMessage,
            CancellationToken ct = default)
            => SubscribeAsync<BitvavoTicker24hMessage>("ticker24h", "ticker24h", symbols, null, (subscribedSymbols, receiveTime, originalData, update) =>
            {
                foreach (var ticker in update.Data)
                {
                    if (subscribedSymbols.Contains(ticker.Symbol))
                        onMessage(CreateUpdate("ticker24h", ticker.Symbol, receiveTime, originalData, ticker, ticker.Timestamp));
                }
            }, ct: ct);

        #endregion

        #region Subscribe To Kline Updates

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(
            string symbol,
            KlineInterval interval,
            Action<DataEvent<BitvavoKlineUpdate>> onMessage,
            CancellationToken ct = default)
            => SubscribeToKlineUpdatesAsync([symbol], interval, onMessage, ct);

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(
            IEnumerable<string> symbols,
            KlineInterval interval,
            Action<DataEvent<BitvavoKlineUpdate>> onMessage,
            CancellationToken ct = default)
            => SubscribeToUpdatesAsync("candles", "candle", symbols, onMessage, update => update.Symbol,
                interval: interval, ct: ct);

        #endregion

        #region Subscribe To Trade Updates

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
            string symbol,
            Action<DataEvent<BitvavoTradeUpdate>> onMessage,
            CancellationToken ct = default)
            => SubscribeToTradeUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
            IEnumerable<string> symbols,
            Action<DataEvent<BitvavoTradeUpdate>> onMessage,
            CancellationToken ct = default)
            => SubscribeToUpdatesAsync("trades", "trade", symbols, onMessage, update => update.Symbol,
                timestamp: update => update.TimestampPrecise ?? update.Timestamp, ct: ct);

        #endregion

        #region Subscribe To OrderBook Updates

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
            string symbol,
            Action<DataEvent<BitvavoOrderBook>> onMessage,
            CancellationToken ct = default)
            => SubscribeToOrderBookUpdatesAsync([symbol], onMessage, ct);

        /// <inheritdoc />
        public Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
            IEnumerable<string> symbols,
            Action<DataEvent<BitvavoOrderBook>> onMessage,
            CancellationToken ct = default)
            => SubscribeToUpdatesAsync("book", "book", symbols, onMessage, update => update.Symbol,
                timestamp: update => update.Timestamp, sequenceNumber: update => update.Nonce, ct: ct);

        #endregion

        private Task<WebSocketResult<UpdateSubscription>> SubscribeToUpdatesAsync<T>(
            string channel,
            string eventName,
            IEnumerable<string> symbols,
            Action<DataEvent<T>> onMessage,
            Func<T, string> getSymbol,
            KlineInterval? interval = null,
            Func<T, DateTime?>? timestamp = null,
            Func<T, long?>? sequenceNumber = null,
            CancellationToken ct = default)
            => SubscribeAsync<T>(channel, eventName, symbols, interval, (_, receiveTime, originalData, update) =>
                onMessage(CreateUpdate(channel, getSymbol(update), receiveTime, originalData, update, timestamp?.Invoke(update), sequenceNumber?.Invoke(update))), ct);

        private Task<WebSocketResult<UpdateSubscription>> SubscribeAsync<T>(
            string channel,
            string eventName,
            IEnumerable<string> symbols,
            KlineInterval? interval,
            Action<HashSet<string>, DateTime, string?, T> handler,
            CancellationToken ct)
        {
            var subscribedSymbols = new HashSet<string>(symbols, StringComparer.Ordinal);
            if (subscribedSymbols.Count == 0)
                return Task.FromResult(WebSocketResult.Fail<UpdateSubscription>(BitvavoExchange.Metadata.Id, ArgumentError.Missing(nameof(symbols))));

            var request = new BitvavoSocketChannel
            {
                Name = channel,
                Symbols = subscribedSymbols.ToArray(),
                Intervals = interval.HasValue ? [interval.Value] : null
            };

            var subscription = new BitvavoSubscription<T>(_baseClient, _logger, request, eventName, (receiveTime, originalData, update) =>
                handler(subscribedSymbols, receiveTime, originalData, update));
            return _baseClient.SubscribeToUpdatesAsync(subscription, ct);
        }

        private DataEvent<T> CreateUpdate<T>(string channel, string symbol, DateTime receiveTime, string? originalData, T update, DateTime? timestamp, long? sequenceNumber = null)
            => new DataEvent<T>(BitvavoExchange.Metadata.Id, update, receiveTime, originalData)
                .WithUpdateType(SocketUpdateType.Update)
                .WithStreamId(channel)
                .WithSymbol(symbol)
                .WithSequenceNumber(sequenceNumber)
                .WithDataTimestamp(timestamp, _baseClient.GetTimeOffset());
    }
}
