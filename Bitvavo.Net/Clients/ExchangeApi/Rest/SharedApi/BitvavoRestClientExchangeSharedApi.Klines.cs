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
        public GetKlinesOptions GetKlinesOptions { get; } = new GetKlinesOptions(_exchangeName, false, true, true, 1440, false,
            SharedKlineInterval.OneMinute,
            SharedKlineInterval.FiveMinutes,
            SharedKlineInterval.FifteenMinutes,
            SharedKlineInterval.ThirtyMinutes,
            SharedKlineInterval.OneHour,
            SharedKlineInterval.TwoHours,
            SharedKlineInterval.FourHours,
            SharedKlineInterval.SixHours,
            SharedKlineInterval.EightHours,
            SharedKlineInterval.TwelveHours,
            SharedKlineInterval.OneDay,
            SharedKlineInterval.OneWeek,
            SharedKlineInterval.OneMonth);

        async Task<IExchangeCallResult<SharedKline[]>> IGetKlines.GetKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetKlinesAsync(request, pageRequest, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedKline[]>> GetKlinesAsync(GetKlinesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetKlinesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedKline[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var endTime = request.EndTime ?? DateTime.UtcNow;
            var page = Pagination.GetPaginationParameters(DataDirection.Descending, request.Limit ?? 1440, request.StartTime, endTime, pageRequest);
            var result = await _api.ExchangeData.GetKlinesAsync(
                symbol, (KlineInterval)request.Interval, page.StartTime, page.EndTime, page.Limit, ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedKline[]>(result);

            var timestamps = result.Data.Select(x => x.OpenTime);
            var next = Pagination.GetNextPageRequest(
                () => new PageRequest { EndTime = timestamps.Min() }, result.Data.Length, timestamps, request.StartTime, endTime, page);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.OpenTime, request.StartTime, request.EndTime, DataDirection.Descending)
                .Select(kline => new SharedKline(
                    request.Symbol, symbol, kline.OpenTime, kline.ClosePrice, kline.HighPrice, kline.LowPrice, kline.OpenPrice,
                    new SharedOrderQuantity(kline.Quantity))).ToArray(), next);
        }
    }
}
