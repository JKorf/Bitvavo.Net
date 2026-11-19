using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BitvavoRestClientExchangeApiAccount : IBitvavoRestClientExchangeApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly BitvavoRestClientExchangeApi _baseClient;

        internal BitvavoRestClientExchangeApiAccount(BitvavoRestClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Account Fees

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoAccountFees>> GetAccountFeesAsync(CancellationToken ct = default)
        {
            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/account", BitvavoExchange.RateLimiter.Bitvavo, 1, true);
            var result = await _baseClient.SendAsync<BitvavoAccountFeesResponse>(request, null, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<BitvavoAccountFees>(result);

            return HttpResult.Ok(result, result.Data.Fees);
        }

        #endregion

        #region Get Balances

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoBalance[]>> GetBalancesAsync(string? asset = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("symbol", asset);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/balance", BitvavoExchange.RateLimiter.Bitvavo, 5, true);
            return await _baseClient.SendAsync<BitvavoBalance[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Staking Balances

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoStakingBalance[]>> GetStakingBalancesAsync(string? asset = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("symbol", asset);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/stakingBalance", BitvavoExchange.RateLimiter.Bitvavo, 5, true);
            return await _baseClient.SendAsync<BitvavoStakingBalance[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Symbol Fees

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoSymbolFees>> GetSymbolFeesAsync(string? symbol = null, FeeQuoteAsset? quoteAsset = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("quote", quoteAsset);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/account/fees", BitvavoExchange.RateLimiter.Bitvavo, 1, true);
            return await _baseClient.SendAsync<BitvavoSymbolFees>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Transaction History

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoTransactionHistory>> GetTransactionHistoryAsync(
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? page = null,
            int? pageSize = null,
            TransactionType? type = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("fromDate", startTime);
            parameters.Add("toDate", endTime);
            parameters.Add("page", page);
            parameters.Add("maxItems", pageSize);
            parameters.Add("type", type);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/account/history", BitvavoExchange.RateLimiter.Bitvavo, 1, true);
            return await _baseClient.SendAsync<BitvavoTransactionHistory>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
