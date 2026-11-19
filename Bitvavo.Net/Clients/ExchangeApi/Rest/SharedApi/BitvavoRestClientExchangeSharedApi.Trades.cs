using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;
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
        private static readonly TimeSpan _tradeHistoryWindow = TimeSpan.FromDays(1);

        public GetRecentTradesOptions GetRecentTradesOptions { get; } = new GetRecentTradesOptions(_exchangeName, 1000, false);
        public GetTradeHistoryOptions GetTradeHistoryOptions { get; } = new GetTradeHistoryOptions(_exchangeName, false, true, true, 1000, false);

        async Task<IExchangeCallResult<SharedTrade[]>> IGetRecentTrades.GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
            => await GetRecentTradesAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedTrade[]>> GetRecentTradesAsync(GetRecentTradesRequest request, CancellationToken ct)
        {
            var validationError = GetRecentTradesOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var result = await _api.ExchangeData.GetTradesAsync(symbol, limit: request.Limit, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTrade[]>(result);

            return HttpResult.Ok(result, result.Data.Select(trade => ParseTrade(request.Symbol, symbol, trade)).ToArray());
        }

        async Task<IExchangeCallResult<SharedTrade[]>> IGetTradeHistory.GetTradeHistoryAsync(GetTradeHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
            => await GetTradeHistoryAsync(request, pageRequest, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedTrade[]>> GetTradeHistoryAsync(GetTradeHistoryRequest request, PageRequest? pageRequest, CancellationToken ct)
        {
            var validationError = GetTradeHistoryOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTrade[]>(Exchange, validationError);

            var symbol = request.Symbol!.GetSymbol(FormatSymbol);
            var endTime = request.EndTime ?? DateTime.UtcNow;
            var page = GetTradeHistoryPage(request.Limit, request.StartTime, endTime, pageRequest);
            var result = await _api.ExchangeData.GetTradesAsync(
                symbol, toId: page.FromId, startTime: page.StartTime, endTime: page.EndTime, limit: page.Limit, ct: ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTrade[]>(result);

            var next = Pagination.GetNextPageRequest(
                () => Pagination.NextPageFromId(result.Data.Last().Id), result.Data.Length,
                result.Data.Select(x => x.Timestamp), request.StartTime, endTime, page, _tradeHistoryWindow);

            return HttpResult.Ok(result, ExchangeHelpers.ApplyFilter(result.Data, x => x.Timestamp, request.StartTime, request.EndTime, DataDirection.Descending)
                .Select(trade => ParseTrade(request.Symbol, symbol, trade)).ToArray(), next);
        }

        private static PaginationParameters GetTradeHistoryPage(int? limit, DateTime? startTime, DateTime endTime, PageRequest? pageRequest)
        {
            var page = Pagination.GetPaginationParameters(
                DataDirection.Descending, limit ?? 500, startTime, endTime, pageRequest, maxPeriod: _tradeHistoryWindow);
            if (startTime > page.StartTime)
                page.StartTime = startTime;

            return page;
        }

        private static SharedTrade ParseTrade(SharedSymbol sharedSymbol, string symbol, BitvavoTrade trade)
            => new SharedTrade(sharedSymbol, symbol, new SharedOrderQuantity(trade.Quantity), trade.Price, trade.Timestamp)
            {
                Side = trade.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell
            };
    }
}
