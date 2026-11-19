using Bitvavo.Net.Enums;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using Bitvavo.Net.Objects.Internal;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BitvavoSocketClientExchangeApiAccount : IBitvavoSocketClientExchangeApiAccount
    {
        private readonly BitvavoSocketClientExchangeApi _baseClient;

        internal BitvavoSocketClientExchangeApiAccount(BitvavoSocketClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Account Fees

        /// <inheritdoc />
        public async Task<QueryResult<BitvavoAccountFees>> GetAccountFeesAsync(CancellationToken ct = default)
        {
            var result = await _baseClient.SendPrivateAsync<BitvavoAccountFeesResponse>("privateGetAccount", null, ct).ConfigureAwait(false);
            if (!result.Success)
                return QueryResult.Fail<BitvavoAccountFees>(result);

            return QueryResult.Ok(result, result.Data.Fees);
        }

        #endregion

        #region Get Balances

        /// <inheritdoc />
        public Task<QueryResult<BitvavoBalance[]>> GetBalancesAsync(string? asset = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("symbol", asset);

            return _baseClient.SendPrivateAsync<BitvavoBalance[]>("privateGetBalance", parameters, ct, weight: 5);
        }

        #endregion

        #region Get Symbol Fees

        /// <inheritdoc />
        public Task<QueryResult<BitvavoSymbolFees>> GetSymbolFeesAsync(string? symbol = null, FeeQuoteAsset? quoteAsset = null, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("market", symbol);
            parameters.Add("quote", quoteAsset);

            return _baseClient.SendPrivateAsync<BitvavoSymbolFees>("privateGetFees", parameters, ct);
        }

        #endregion

        #region Get Transaction History

        /// <inheritdoc />
        public Task<QueryResult<BitvavoTransactionHistory>> GetTransactionHistoryAsync(
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

            return _baseClient.SendPrivateAsync<BitvavoTransactionHistory>("privateGetTransactionHistory", parameters, ct);
        }

        #endregion

        #region Get Deposit Data

        /// <inheritdoc />
        public async Task<QueryResult<BitvavoDepositData>> GetDepositDataAsync(string asset, CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("symbol", asset);

            var result = await _baseClient.SendPrivateAsync<BitvavoSocketDepositData>("privateDepositAssets", parameters, ct).ConfigureAwait(false);
            if (!result.Success)
                return QueryResult.Fail<BitvavoDepositData>(result);

            return QueryResult.Ok<BitvavoDepositData>(result, result.Data);
        }

        #endregion

        #region Get Deposit History

        /// <inheritdoc />
        public Task<QueryResult<BitvavoDeposit[]>> GetDepositHistoryAsync(
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

            return _baseClient.SendPrivateAsync<BitvavoDeposit[]>("privateGetDepositHistory", parameters, ct, weight: 5);
        }

        #endregion

        #region Withdraw

        /// <inheritdoc />
        public Task<QueryResult<BitvavoWithdrawalResult>> WithdrawAsync(
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

            return _baseClient.SendPrivateAsync<BitvavoWithdrawalResult>("privateWithdrawAssets", parameters, ct);
        }

        #endregion

        #region Withdraw Crypto

        /// <inheritdoc />
        public Task<QueryResult<BitvavoCryptoWithdrawal>> WithdrawCryptoAsync(
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

            return _baseClient.SendPrivateAsync<BitvavoCryptoWithdrawal>("privateCryptoWithdrawal", parameters, ct, weight: 25);
        }

        #endregion

        #region Get Withdrawal History

        /// <inheritdoc />
        public Task<QueryResult<BitvavoWithdrawal[]>> GetWithdrawalHistoryAsync(
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

            return _baseClient.SendPrivateAsync<BitvavoWithdrawal[]>("privateGetWithdrawalHistory", parameters, ct, weight: 5);
        }

        #endregion
    }
}
