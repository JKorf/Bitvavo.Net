using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models.Spot;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Interfaces.Clients.SpotApi;

/// <summary>
/// Bitvavo Institutional REST endpoints and subaccount management, asset transfers between
/// the main account and subaccounts, and per-subaccount balance / order / transaction
/// queries.
/// </summary>
public interface IBitvavoRestClientSpotApiInstitutional
{
    /// <summary>
    /// Create a new subaccount under the main account.
    /// <para><a href="https://docs.bitvavo.com/docs/institutional-api/create-subaccount">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="label">Optional description of the subaccount (up to 100 characters).</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoSubaccount>> CreateSubaccountAsync(string? label = null, CancellationToken ct = default);

    /// <summary>
    /// Get the subaccounts under the main account, paginated by page number.
    /// <para><a href="https://docs.bitvavo.com/docs/institutional-api/get-subaccounts">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="page">Page</param>
    /// <param name="pageSize">Page size</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoSubaccountList>> GetSubaccountsAsync(int? page = null, int? pageSize = null, CancellationToken ct = default);

    /// <summary>
    /// Transfer an asset between the main account and a subaccount.
    /// <para><a href="https://docs.bitvavo.com/docs/institutional-api/create-transfer">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="subaccountId">Sub account id</param>
    /// <param name="direction">Transfer direction</param>
    /// <param name="asset">Asset symbol to transfer (e.g. <c>"EUR"</c>, <c>"BTC"</c>)</param>
    /// <param name="quantity">Quantity to transfer</param>
    /// <param name="clientOrderId">Client order id</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoSubaccountTransfer>> CreateTransferAsync(
        string subaccountId,
        SubaccountTransferDirection direction,
        string asset,
        decimal quantity,
        string? clientOrderId = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get a single subaccount transfer
    /// <para><a href="https://docs.bitvavo.com/docs/institutional-api/get-transfer">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="transferId">Transfer id</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoSubaccountTransfer>> GetTransferAsync(string transferId, CancellationToken ct = default);

    /// <summary>
    /// Get subaccount transfers for a single subaccount, optionally filtered by time window.
    /// <para><a href="https://docs.bitvavo.com/docs/institutional-api/get-transfers">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="subaccountId">Sub account id</param>
    /// <param name="clientRequestId">Filter by client order id</param>
    /// <param name="startTime">Inclusive UTC lower bound.</param>
    /// <param name="endTime">Inclusive UTC upper bound.</param>
    /// <param name="limit">Maximum entries, max 1000</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoSubaccountTransferList>> GetTransfersAsync(
        string subaccountId,
        string? clientRequestId = null,
        DateTime? startTime = null,
        DateTime? endTime = null,
        int? limit = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get the balances of a subaccount, or of the main account when
    /// <paramref name="subaccountId"/> is null.
    /// <para><a href="https://docs.bitvavo.com/docs/institutional-api/get-balance">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="subaccountId">Sub account id</param>
    /// <param name="asset">Optional asset filter</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoSubaccountBalances>> GetSubaccountBalancesAsync(
        string? subaccountId = null,
        string? asset = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get the transaction history of a subaccount, paginated by page number.
    /// <para><a href="https://docs.bitvavo.com/docs/institutional-api/get-transaction-history">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="subaccountId">Sub account id</param>
    /// <param name="fromDate">Inclusive UTC lower bound.</param>
    /// <param name="toDate">Inclusive UTC upper bound.</param>
    /// <param name="page">Page</param>
    /// <param name="pageSize">Page size, max 100</param>
    /// <param name="type">Optional transaction-type filter (e.g. <c>"buy"</c>, <c>"deposit"</c>).</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoTransactionHistory>> GetSubaccountTransactionHistoryAsync(
        string? subaccountId = null,
        DateTime? fromDate = null,
        DateTime? toDate = null,
        int? page = null,
        int? pageSize = null,
        string? type = null,
        CancellationToken ct = default);

    /// <summary>
    /// Get the open orders of a subaccount, or of the main account when
    /// <paramref name="subaccountId"/> is null.
    /// <para><a href="https://docs.bitvavo.com/docs/institutional-api/get-open-orders">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="subaccountId">Sub account id</param>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="baseAsset">Optional base-asset filter (e.g. <c>"BTC"</c>).</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<IEnumerable<BitvavoOrder>>> GetSubaccountOpenOrdersAsync(
        string? subaccountId = null,
        string? symbol = null,
        string? baseAsset = null,
        CancellationToken ct = default);

    /// <summary>
    /// Cancel a single order on a subaccount (or the main account).
    /// <para><a href="https://docs.bitvavo.com/docs/institutional-api/cancel-order">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="orderId">Order id</param>
    /// <param name="operatorId">Operator id</param>
    /// <param name="subaccountId">Sub account id</param>
    /// <param name="clientOrderId">Client order id</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<BitvavoOrderId>> CancelSubaccountOrderAsync(
        long operatorId,
        string symbol,
        string orderId,
        string? subaccountId = null,
        string? clientOrderId = null,
        CancellationToken ct = default);

    /// <summary>
    /// Cancel all open orders on a subaccount (or the main account)
    /// <para><a href="https://docs.bitvavo.com/docs/institutional-api/cancel-orders">Bitvavo API docs</a></para>
    /// </summary>
    /// <param name="operatorId">Operator id</param>
    /// <param name="subaccountId">Sub account id</param>
    /// <param name="symbol">Symbol name, for example `ETH-EUR`</param>
    /// <param name="ct">Cancellation token.</param>
    Task<HttpResult<IEnumerable<BitvavoOrderId>>> CancelSubaccountOrdersAsync(
        long operatorId,
        string? subaccountId = null,
        string? symbol = null,
        CancellationToken ct = default);
}
