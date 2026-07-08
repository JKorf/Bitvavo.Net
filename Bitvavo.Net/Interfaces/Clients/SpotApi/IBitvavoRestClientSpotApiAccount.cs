using Bitvavo.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Interfaces.Clients.SpotApi;

/// <summary>
/// Bitvavo signed account-data REST endpoints
/// </summary>
public interface IBitvavoRestClientSpotApiAccount
{
    /// <summary>
    /// Get account info
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-account-balance">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoAccountInfo>> GetAccountInfoAsync(CancellationToken ct = default);

    /// <summary>
    /// Get account balances
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-account-balance">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="asset">Optional asset symbol (e.g. <c>"BTC"</c>) to filter the result. Null returns all assets.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoBalance[]>> GetBalancesAsync(string? asset = null, CancellationToken ct = default);

    /// <summary>
    /// Get the market-specific fee schedule for the active 30-day-volume tier.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-account-fees">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoMarketFee>> GetTradingFeesAsync(string? symbol = null, CancellationToken ct = default);

    /// <summary>
    /// Set or refresh the server-side cancel-on-disconnect deadline for orders tagged with
    /// <paramref name="codGroupId"/>.
    /// <para><a href="https://docs.bitvavo.com/docs/cancel-on-disconnect">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="codGroupId">Numeric cancel-on-disconnect group identifier.</param>
    /// <param name="expiryAfterSeconds">Seconds until the broker cancels the group (minimum 10; 0 removes the group).</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoCancelOrdersAfter>> ResetCancelOnDisconnectAsync(
        int codGroupId, int expiryAfterSeconds, CancellationToken ct = default);

    /// <summary>
    /// Get the account transaction history (the account ledger)
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-transaction-history">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="fromDate">Inclusive UTC lower bound on transaction execution time.</param>
    /// <param name="toDate">Inclusive UTC upper bound on transaction execution time.</param>
    /// <param name="page">Page</param>
    /// <param name="pageSize">Maximum number of items per page, max 100</param>
    /// <param name="type">Optional transaction-type filter, one of <c>sell</c>, <c>buy</c>, <c>staking</c>, <c>fixed_staking</c>, <c>deposit</c>, <c>withdrawal</c>, <c>affiliate</c>, <c>distribution</c>, <c>internal_transfer</c>, <c>withdrawal_cancelled</c>, <c>rebate</c>, <c>loan</c>, <c>external_transferred_funds</c>, <c>manually_assigned</c>.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoTransactionHistory>> GetTransactionHistoryAsync(
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? page = null,
        int? pageSize = null,
        string? type = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get the deposit address (or fiat banking instructions) for a given asset.
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-deposit-data">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="asset">Asset symbol (e.g. <c>"BTC"</c>, <c>"EUR"</c>)</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoDepositAddress>> GetDepositAddressAsync(string asset, CancellationToken ct = default);

    /// <summary>
    /// Get the deposit history for the account
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-deposit-history">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="asset">Optional asset filter.</param>
    /// <param name="limit">Maximum entries, max 1000</param>
    /// <param name="startTime">Inclusive UTC lower bound.</param>
    /// <param name="endTime">Inclusive UTC upper bound.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoDepositHistoryEntry[]>> GetDepositHistoryAsync(
        string? asset = null,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get the withdrawal history for the account
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/get-withdrawal-history">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="asset">Optional asset filter.</param>
    /// <param name="limit">Maximum entries, max 1000</param>
    /// <param name="startTime">Inclusive UTC lower bound.</param>
    /// <param name="endTime">Inclusive UTC upper bound.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoWithdrawalHistoryEntry[]>> GetWithdrawalHistoryAsync(
        string? asset = null,
        int? limit = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        CancellationToken ct = default);

    /// <summary>
    /// Submit a withdrawal request
    /// <para><a href="https://docs.bitvavo.com/docs/rest-api/withdraw-assets">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="asset">Asset to withdraw (e.g. <c>"BTC"</c>, <c>"EUR"</c>)</param>
    /// <param name="quantity">Quantity of the asset to withdraw, in base-asset units</param>
    /// <param name="address">Destination wallet address (crypto) or IBAN (fiat)</param>
    /// <param name="paymentId">Memo / payment reference for assets that require it.</param>
    /// <param name="addWithdrawalFee">If true, fee is added on top of <paramref name="quantity"/>; otherwise fee is deducted from <paramref name="quantity"/>.</param>
    /// <param name="internalTransfer">If true, transfer between Bitvavo accounts.</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoWithdrawalResult>> WithdrawAsync(
        string asset,
        decimal quantity,
        string address,
        string? paymentId = null,
        bool? addWithdrawalFee = null,
        bool? internalTransfer = null,
        CancellationToken ct = default);
}
