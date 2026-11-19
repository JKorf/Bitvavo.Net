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
            TransactionType? type = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? page = null,
            int? pageSize = null,
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

        #region Get Deposit Data

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoDepositData>> GetDepositDataAsync(string asset, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("symbol", asset);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/deposit", BitvavoExchange.RateLimiter.Bitvavo, 1, true);
            return await _baseClient.SendAsync<BitvavoDepositData>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Deposit History

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoDeposit[]>> GetDepositHistoryAsync(
            string? asset = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("symbol", asset);
            parameters.Add("start", startTime);
            parameters.Add("end", endTime);
            parameters.Add("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/depositHistory", BitvavoExchange.RateLimiter.Bitvavo, 5, true);
            return await _baseClient.SendAsync<BitvavoDeposit[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Withdraw

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoWithdrawalResult>> WithdrawAsync(
            string asset,
            string address,
            decimal quantity,
            string? paymentId = null,
            bool? addWithdrawalFee = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("symbol", asset);
            parameters.Add("address", address);
            parameters.Add("amount", quantity);
            parameters.Add("paymentId", paymentId);
            parameters.Add("addWithdrawalFee", addWithdrawalFee);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/withdrawal", BitvavoExchange.RateLimiter.Bitvavo, 1, true);
            return await _baseClient.SendAsync<BitvavoWithdrawalResult>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Withdraw Crypto

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoCryptoWithdrawal>> WithdrawCryptoAsync(
            string asset,
            string network,
            string address,
            decimal quantity,
            string? memo = null,
            bool? deductFeeFromQuantity = null,
            string? idempotencyKey = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("asset", asset);
            parameters.Add("network", network);
            parameters.Add("address", address);
            parameters.Add("amount", quantity);
            parameters.Add("memo", memo);
            parameters.Add("deductFeeFromAmount", deductFeeFromQuantity);
            parameters.Add("idempotencyKey", idempotencyKey);

            var request = _definitions.GetOrCreate(HttpMethod.Post, _baseClient.BaseAddress, "/v2/crypto/withdrawal", BitvavoExchange.RateLimiter.Bitvavo, 25, true);
            return await _baseClient.SendAsync<BitvavoCryptoWithdrawal>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion

        #region Get Withdrawal History

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoWithdrawal[]>> GetWithdrawalHistoryAsync(
            string? asset = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("symbol", asset);
            parameters.Add("start", startTime);
            parameters.Add("end", endTime);
            parameters.Add("limit", limit);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, "/v2/withdrawalHistory", BitvavoExchange.RateLimiter.Bitvavo, 5, true);
            return await _baseClient.SendAsync<BitvavoWithdrawal[]>(request, parameters, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
