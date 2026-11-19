using Bitvavo.Net.Enums;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BitvavoSocketClientExchangeApiExchangeData : IBitvavoSocketClientExchangeApiExchangeData
    {
        private readonly BitvavoSocketClientExchangeApi _baseClient;

        internal BitvavoSocketClientExchangeApiExchangeData(BitvavoSocketClientExchangeApi baseClient)
        {
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
    }
}
