using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BitvavoRestClientExchangeApiExchangeData : IBitvavoRestClientExchangeApiExchangeData
    {
        private readonly BitvavoRestClientExchangeApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal BitvavoRestClientExchangeApiExchangeData(ILogger logger, BitvavoRestClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Server Time

        /// <inheritdoc />
        public async Task<HttpResult<DateTime>> GetServerTimeAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/time", BitvavoExchange.RateLimiter.Bitvavo, 1, false, preventCaching: true);
            var result = await _baseClient.SendAsync<BitvavoServerTime>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<DateTime>(result);

            return HttpResult.Ok(result, result.Data.Time);
        }

        #endregion

        #region Get Symbol

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoSymbol>> GetSymbolAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/markets", BitvavoExchange.RateLimiter.Bitvavo, 1, false);
            return await _baseClient.SendAsync<BitvavoSymbol>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Symbols

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoSymbol[]>> GetSymbolsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/markets", BitvavoExchange.RateLimiter.Bitvavo, 1, false);
            return await _baseClient.SendAsync<BitvavoSymbol[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Asset

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoAsset>> GetAssetAsync(string asset, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("symbol", asset);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/assets", BitvavoExchange.RateLimiter.Bitvavo, 1, false);
            return await _baseClient.SendAsync<BitvavoAsset>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Assets

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoAsset[]>> GetAssetsAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/assets", BitvavoExchange.RateLimiter.Bitvavo, 1, false);
            return await _baseClient.SendAsync<BitvavoAsset[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Trades

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoTrade[]>> GetTradesAsync(
            string symbol,
            string? fromId = null,
            string? toId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("limit", limit);
            parameters.Add("start", startTime);
            parameters.Add("end", endTime);
            parameters.Add("tradeIdFrom", fromId);
            parameters.Add("tradeIdTo", toId);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, $"/v2/{symbol}/trades", BitvavoExchange.RateLimiter.Bitvavo, 5, false);
            return await _baseClient.SendAsync<BitvavoTrade[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Book

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoOrderBook>> GetOrderBookAsync(string symbol, int? depth = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("depth", depth);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, $"/v2/{symbol}/book", BitvavoExchange.RateLimiter.Bitvavo, 1, false);
            return await _baseClient.SendAsync<BitvavoOrderBook>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Order Book Report

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoOrderBookReport>> GetOrderBookReportAsync(string symbol, int? depth = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("depth", depth);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, $"/v2/report/{symbol}/book", BitvavoExchange.RateLimiter.Bitvavo, 1, false);
            return await _baseClient.SendAsync<BitvavoOrderBookReport>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Trades Report

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoTradeReport[]>> GetTradesReportAsync(
            string symbol,
            string? fromId = null,
            string? toId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("tradeIdFrom", fromId);
            parameters.Add("tradeIdTo", toId);
            parameters.Add("start", startTime);
            parameters.Add("end", endTime);
            parameters.Add("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, $"/v2/report/{symbol}/trades", BitvavoExchange.RateLimiter.Bitvavo, 5, false);
            return await _baseClient.SendAsync<BitvavoTradeReport[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Klines

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoKline[]>> GetKlinesAsync(
            string symbol,
            KlineInterval interval,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("interval", interval);
            parameters.Add("start", startTime);
            parameters.Add("end", endTime);
            parameters.Add("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, $"/v2/{symbol}/candles", BitvavoExchange.RateLimiter.Bitvavo, 1, false);
            return await _baseClient.SendAsync<BitvavoKline[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Ticker Price

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoTickerPrice>> GetTickerPriceAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/ticker/price", BitvavoExchange.RateLimiter.Bitvavo, 1, false);
            return await _baseClient.SendAsync<BitvavoTickerPrice>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Ticker Prices

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoTickerPrice[]>> GetTickerPricesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/ticker/price", BitvavoExchange.RateLimiter.Bitvavo, 1, false);
            return await _baseClient.SendAsync<BitvavoTickerPrice[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Book Ticker

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoBookTicker>> GetBookTickerAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/ticker/book", BitvavoExchange.RateLimiter.Bitvavo, 1, false);
            return await _baseClient.SendAsync<BitvavoBookTicker>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Book Tickers

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoBookTicker[]>> GetBookTickersAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/ticker/book", BitvavoExchange.RateLimiter.Bitvavo, 1, false);
            return await _baseClient.SendAsync<BitvavoBookTicker[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Ticker

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoTicker>> GetTickerAsync(string symbol, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/ticker/24h", BitvavoExchange.RateLimiter.Bitvavo, 25, false);
            return await _baseClient.SendAsync<BitvavoTicker>(request, parameters, ct, weight: 1).ConfigureAwait(false);
        }

        #endregion

        #region Get Tickers

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoTicker[]>> GetTickersAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/ticker/24h", BitvavoExchange.RateLimiter.Bitvavo, 25, false);
            return await _baseClient.SendAsync<BitvavoTicker[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
