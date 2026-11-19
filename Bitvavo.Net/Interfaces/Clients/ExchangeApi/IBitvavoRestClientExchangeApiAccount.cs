using System;
using System.Threading;
using System.Threading.Tasks;
using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// Bitvavo account data endpoints
    /// </summary>
    public interface IBitvavoRestClientExchangeApiAccount
    {
        /// <summary>
        /// Get account trading fees
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-account-fees/" /><br />
        /// Endpoint:<br />
        /// GET /v2/account
        /// </para>
        /// </summary>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoAccountFees>> GetAccountFeesAsync(CancellationToken ct = default);

        /// <summary>
        /// Get account balances
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-account-balance/" /><br />
        /// Endpoint:<br />
        /// GET /v2/balance
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>symbol</c>"] Asset filter</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoBalance[]>> GetBalancesAsync(string? asset = null, CancellationToken ct = default);

        /// <summary>
        /// Get balances locked in fixed staking
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-staking-balance/" /><br />
        /// Endpoint:<br />
        /// GET /v2/stakingBalance
        /// </para>
        /// </summary>
        /// <param name="asset">["<c>symbol</c>"] Asset filter</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoStakingBalance[]>> GetStakingBalancesAsync(string? asset = null, CancellationToken ct = default);

        /// <summary>
        /// Get trading fees for a symbol or quote asset
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-market-fees/" /><br />
        /// Endpoint:<br />
        /// GET /v2/account/fees
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Symbol filter</param>
        /// <param name="quoteAsset">["<c>quote</c>"] Quote asset filter</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoSymbolFees>> GetSymbolFeesAsync(string? symbol = null, FeeQuoteAsset? quoteAsset = null, CancellationToken ct = default);

        /// <summary>
        /// Get account transaction history
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-transaction-history/" /><br />
        /// Endpoint:<br />
        /// GET /v2/account/history
        /// </para>
        /// </summary>
        /// <param name="type">["<c>type</c>"] Transaction type filter. ManuallyAssignedBitvavo is only returned in responses</param>
        /// <param name="startTime">["<c>fromDate</c>"] Start time</param>
        /// <param name="endTime">["<c>toDate</c>"] End time</param>
        /// <param name="page">["<c>page</c>"] Page number, starting at 1</param>
        /// <param name="pageSize">["<c>maxItems</c>"] Maximum transactions per page, up to 100</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoTransactionHistory>> GetTransactionHistoryAsync(
            TransactionType? type = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? page = null,
            int? pageSize = null,
            CancellationToken ct = default);
    }
}
