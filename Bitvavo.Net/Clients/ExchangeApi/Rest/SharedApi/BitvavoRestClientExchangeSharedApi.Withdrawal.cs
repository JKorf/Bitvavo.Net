using Bitvavo.Net.Enums;
using CryptoExchange.Net;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    internal partial class BitvavoRestClientExchangeSharedApi
    {
        public WithdrawOptions WithdrawOptions { get; } = new WithdrawOptions(_exchangeName)
        {
            ParameterRuleOverrides = [RequestParameterRuleOverride<WithdrawRequest>.Required(x => x.Network)],
            ExchangeParameterRules = [ExchangeParameterRule.Optional("idempotencyKey", "Reuse the same key when retrying a withdrawal", "withdrawal-123")]
        };

        public GetWithdrawalHistoryOptions GetWithdrawalHistoryOptions { get; } = new GetWithdrawalHistoryOptions(_exchangeName, false, true, true, 1000);

        async Task<IExchangeCallResult<SharedId>> IWithdraw.WithdrawAsync(WithdrawRequest request, CancellationToken ct)
            => await WithdrawAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedId>> WithdrawAsync(WithdrawRequest request, CancellationToken ct)
        {
            var validationError = WithdrawOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedId>(Exchange, validationError);

            var result = await _api.Account.WithdrawCryptoAsync(
                request.Asset, request.Network!, request.Address, request.Quantity, memo: request.AddressTag,
                idempotencyKey: request.GetParamValue<string>(_exchangeName, "idempotencyKey"), ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedId>(result);

            return HttpResult.Ok(result, new SharedId(result.Data.Id));
        }

        async Task<IExchangeCallResult<SharedWithdrawal[]>> IGetWithdrawalHistory.GetWithdrawalHistoryAsync(GetWithdrawalsRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetWithdrawalHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedWithdrawal[]>> GetWithdrawalHistoryAsync(GetWithdrawalsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetWithdrawalHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedWithdrawal[]>(Exchange, validationError);

            var endTime = request.EndTime ?? DateTime.UtcNow;
            var page = Pagination.GetPaginationParameters(DataDirection.Descending, request.Limit ?? 500, request.StartTime, endTime, pageRequest);
            var result = await _api.Account.GetWithdrawalHistoryAsync(request.Asset, page.StartTime, page.EndTime, page.Limit, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedWithdrawal[]>(result);

            var timestamps = result.Data.Select(x => x.Timestamp);
            var next = Pagination.GetNextPageRequest(
                () => new PageRequest { EndTime = timestamps.Min() }, result.Data.Length, timestamps, request.StartTime, endTime, page);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, DataDirection.Descending)
                .Select(withdrawal =>
                {
                    var status = withdrawal.Status switch
                    {
                        WithdrawalStatus.Completed => SharedTransferStatus.Completed,
                        WithdrawalStatus.Canceled => SharedTransferStatus.Failed,
                        _ => SharedTransferStatus.InProgress
                    };
                    return new SharedWithdrawal(
                        withdrawal.Asset, withdrawal.Address ?? string.Empty, withdrawal.Quantity,
                        status == SharedTransferStatus.Completed, withdrawal.Timestamp, status)
                    {
                        TransactionId = withdrawal.TransactionId,
                        Tag = withdrawal.PaymentId,
                        Fee = withdrawal.Fee
                    };
                }).ToArray(), next);
        }
    }
}
