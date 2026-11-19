using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bitvavo.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// Bitvavo WebSocket account endpoints
    /// </summary>
    public interface IBitvavoSocketClientExchangeApiAccount
    {
        /// <summary>
        /// Get account trading fees
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-account-fees/" /><br />
        /// Endpoint:<br />
        /// privateGetAccount
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoAccountFees>> GetAccountFeesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get account balances
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-account-balance/" /><br />
        /// Endpoint:<br />
        /// privateGetBalance
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>symbol</c>"] Asset filter</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoBalance[]>> GetBalancesAsync(string? asset = null, CancellationToken ct = default);

        /// <summary>
        /// Get trading fees for a symbol or quote asset
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-market-fees/" /><br />
        /// Endpoint:<br />
        /// privateGetFees
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol filter</param>
        /// <param name="quoteAsset">["<c>quote</c>"] Quote asset filter</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoSymbolFees>> GetSymbolFeesAsync(string? symbol = null, FeeQuoteAsset? quoteAsset = null, CancellationToken ct = default);

        /// <summary>
        /// Get account transaction history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-transaction-history/" /><br />
        /// Endpoint:<br />
        /// privateGetTransactionHistory
        /// </para>
        /// </summary>
        /// <param name="type">["<c>type</c>"] Transaction type filter. ManuallyAssignedBitvavo is only returned in responses</param>
        /// <param name="startTime">["<c>fromDate</c>"] Start time</param>
        /// <param name="endTime">["<c>toDate</c>"] End time</param>
        /// <param name="page">["<c>page</c>"] Page number, starting at 1</param>
        /// <param name="pageSize">["<c>maxItems</c>"] Maximum transactions per page, up to 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoTransactionHistory>> GetTransactionHistoryAsync(
            TransactionType? type = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? page = null,
            int? pageSize = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get wallet or bank details for depositing an asset
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-deposit-data/" /><br />
        /// Endpoint:<br />
        /// privateDepositAssets
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>symbol</c>"] Asset to transfer</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoDepositData>> GetDepositDataAsync(string asset, CancellationToken ct = default);

        /// <summary>
        /// Get account deposit history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-deposit-history/" /><br />
        /// Endpoint:<br />
        /// privateGetDepositHistory
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>symbol</c>"] Asset filter</param>
        /// <param name="startTime">["<c>start</c>"] Start time</param>
        /// <param name="endTime">["<c>end</c>"] End time</param>
        /// <param name="limit">["<c>limit</c>"] Maximum number of results, up to 1000</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoDeposit[]>> GetDepositHistoryAsync(
            string? asset = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);

        /// <summary>
        /// Withdraw an asset to a wallet or bank account
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/withdraw-assets/" /><br />
        /// Endpoint:<br />
        /// privateWithdrawAssets
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>symbol</c>"] Asset to transfer</param>
        /// <param name="address">["<c>address</c>"] Destination address</param>
        /// <param name="quantity">["<c>amount</c>"] Quantity to withdraw</param>
        /// <param name="paymentId">["<c>paymentId</c>"] Destination memo or tag</param>
        /// <param name="addWithdrawalFee">["<c>addWithdrawalFee</c>"] Charge the fee in addition to the withdrawal quantity</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoWithdrawalResult>> WithdrawAsync(
            string asset,
            string address,
            decimal quantity,
            string? paymentId = null,
            bool? addWithdrawalFee = null,
            CancellationToken ct = default);

        /// <summary>
        /// Withdraw a crypto asset over a specified network
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/withdraw-crypto-assets/" /><br />
        /// Endpoint:<br />
        /// privateCryptoWithdrawal
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>asset</c>"] Asset to withdraw</param>
        /// <param name="network">["<c>network</c>"] Withdrawal network</param>
        /// <param name="address">["<c>address</c>"] Destination address</param>
        /// <param name="quantity">["<c>amount</c>"] Quantity to withdraw</param>
        /// <param name="memo">["<c>memo</c>"] Destination memo or tag required by the network</param>
        /// <param name="deductFeeFromQuantity">["<c>deductFeeFromAmount</c>"] Deduct the fee from the withdrawal quantity</param>
        /// <param name="idempotencyKey">["<c>idempotencyKey</c>"] Key of 5 to 50 characters used to retry the same withdrawal</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoCryptoWithdrawal>> WithdrawCryptoAsync(
            string asset,
            string network,
            string address,
            decimal quantity,
            string? memo = null,
            bool? deductFeeFromQuantity = null,
            string? idempotencyKey = null,
            CancellationToken ct = default);

        /// <summary>
        /// Get account withdrawal history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/websocket-api/get-withdrawal-history/" /><br />
        /// Endpoint:<br />
        /// privateGetWithdrawalHistory
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>symbol</c>"] Asset filter</param>
        /// <param name="startTime">["<c>start</c>"] Start time</param>
        /// <param name="endTime">["<c>end</c>"] End time</param>
        /// <param name="limit">["<c>limit</c>"] Maximum number of results, up to 1000</param>
        /// <param name="ct">Cancellation token</param>
        Task<QueryResult<BitvavoWithdrawal[]>> GetWithdrawalHistoryAsync(
            string? asset = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);
    }
}
