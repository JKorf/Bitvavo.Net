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
        public GetSpotUserTradeHistoryOptions GetSpotUserTradeHistoryOptions { get; } = new GetSpotUserTradeHistoryOptions(_exchangeName, false, true, true, 1000);
        public GetSpotOrderTradesOptions GetSpotOrderTradesOptions { get; } = new GetSpotOrderTradesOptions(_exchangeName, true);

        async Task<IExchangeCallResult<SharedUserTrade[]>> IGetSpotUserTradeHistory.GetSpotUserTradeHistoryAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetSpotUserTradeHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedUserTrade[]>> GetSpotUserTradeHistoryAsync(GetUserTradesRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetSpotUserTradeHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var endTime = request.EndTime ?? DateTime.UtcNow;
            var useTimeFilter = request.StartTime.HasValue || request.EndTime.HasValue;
            var page = useTimeFilter
                ? GetTradeHistoryPage(request.Limit, request.StartTime, endTime, pageRequest)
                : Pagination.GetPaginationParameters(DataDirection.Descending, request.Limit ?? 500, null, endTime, pageRequest);
            var result = await _api.Trading.GetUserTradesAsync(
                symbol, toId: page.FromId, startTime: page.StartTime, endTime: useTimeFilter ? page.EndTime : null, limit: page.Limit, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedUserTrade[]>(result);

            var next = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromId(result.Data.Last().Id), result.Data.Length,
                result.Data.Select(x => x.Timestamp), request.StartTime, endTime, page, request.StartTime.HasValue ? _tradeHistoryWindow : null);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, DataDirection.Descending)
                .Select(trade => new SharedUserTrade(
                    request.Symbol, symbol, trade.OrderId, trade.Id,
                    (trade.Side == OrderSide.Buy) == trade.IsTaker ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                    new SharedOrderQuantity(trade.Quantity), trade.Price, trade.Timestamp)
                {
                    ClientOrderId = trade.ClientOrderId,
                    Fee = trade.FeeQuantity,
                    FeeAsset = trade.FeeAsset,
                    Role = trade.IsTaker ? SharedRole.Taker : SharedRole.Maker
                }).ToArray(), next);
        }

        async Task<IExchangeCallResult<SharedUserTrade[]>> IGetSpotOrderTrades.GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
            => await GetSpotOrderTradesAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedUserTrade[]>> GetSpotOrderTradesAsync(GetOrderTradesRequest request, CancellationToken ct)
        {
            var validationError = GetSpotOrderTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedUserTrade[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.Trading.GetOrderAsync(symbol, request.OrderId, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedUserTrade[]>(result);

            var order = result.Data;
            var fills = order.Fills?.Select(fill => new SharedUserTrade(
                request.Symbol, symbol, order.Id, fill.Id,
                order.Side == null ? null : order.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                new SharedOrderQuantity(fill.Quantity), fill.Price, fill.Timestamp)
            {
                ClientOrderId = order.ClientOrderId,
                Fee = fill.FeeQuantity,
                FeeAsset = fill.FeeAsset,
                Role = fill.IsTaker ? SharedRole.Taker : SharedRole.Maker
            }).ToArray() ?? [];
            return HttpResult.Ok(result, fills);
        }
    }
}
