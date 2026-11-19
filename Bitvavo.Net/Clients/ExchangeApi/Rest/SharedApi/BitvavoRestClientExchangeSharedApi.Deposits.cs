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
        public GetDepositAddressesOptions GetDepositAddressesOptions { get; } = new GetDepositAddressesOptions(_exchangeName, true)
        {
            ParameterRuleOverrides = [RequestParameterRuleOverride<GetDepositAddressesRequest>.NotSupported(x => x.Network)]
        };

        public GetDepositHistoryOptions GetDepositHistoryOptions { get; } = new GetDepositHistoryOptions(_exchangeName, false, true, true, 1000);

        async Task<IExchangeCallResult<SharedDepositAddress[]>> IGetDepositAddresses.GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
            => await GetDepositAddressesAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedDepositAddress[]>> GetDepositAddressesAsync(GetDepositAddressesRequest request, CancellationToken ct)
        {
            var validationError = GetDepositAddressesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedDepositAddress[]>(Exchange, validationError);

            var result = await _api.Account.GetDepositDataAsync(request.Asset, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedDepositAddress[]>(result);

            var data = result.Data;
            var addresses = string.IsNullOrEmpty(data.Address)
                ? Array.Empty<SharedDepositAddress>()
                : new[] { new SharedDepositAddress(request.Asset, data.Address!) { TagOrMemo = data.PaymentId } };
            return HttpResult.Ok(result, addresses);
        }

        async Task<IExchangeCallResult<SharedDeposit[]>> IGetDepositHistory.GetDepositHistoryAsync(GetDepositsRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetDepositHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedDeposit[]>> GetDepositHistoryAsync(GetDepositsRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetDepositHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedDeposit[]>(Exchange, validationError);

            var endTime = request.EndTime ?? DateTime.UtcNow;
            var page = Pagination.GetPaginationParameters(DataDirection.Descending, request.Limit ?? 500, request.StartTime, endTime, pageRequest);
            var result = await _api.Account.GetDepositHistoryAsync(request.Asset, page.StartTime, page.EndTime, page.Limit, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedDeposit[]>(result);

            var timestamps = result.Data.Select(x => x.Timestamp);
            var next = Pagination.GetNextPageRequest(
                () => new PageRequest { EndTime = timestamps.Min() }, result.Data.Length, timestamps, request.StartTime, endTime, page);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, DataDirection.Descending)
                .Select(deposit =>
                {
                    var status = deposit.Status switch
                    {
                        DepositStatus.Completed => SharedTransferStatus.Completed,
                        DepositStatus.Canceled => SharedTransferStatus.Failed,
                        _ => SharedTransferStatus.Unknown
                    };
                    return new SharedDeposit(deposit.Asset, deposit.Quantity, status == SharedTransferStatus.Completed, deposit.Timestamp, status)
                    {
                        TransactionId = deposit.TransactionId,
                        Tag = deposit.PaymentId
                    };
                }).ToArray(), next);
        }
    }
}
