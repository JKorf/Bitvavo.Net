using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.SharedApis;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    internal partial class BitvavoRestClientExchangeSharedApi
    {
        public GetTickerOptions GetTickerOptions { get; } = new GetTickerOptions(_exchangeName);
        public GetAllTickersOptions GetAllTickersOptions { get; } = new GetAllTickersOptions(_exchangeName);

        async Task<IExchangeCallResult<SharedTicker>> IGetTicker.GetTickerAsync(GetTickerRequest request, CancellationToken ct)
            => await GetTickerAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedTicker>> GetTickerAsync(GetTickerRequest request, CancellationToken ct)
        {
            var validationError = GetTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTicker>(Exchange, validationError);

            var result = await _api.ExchangeData.GetTickerAsync(request.Symbol!.GetSymbol(FormatSymbol), ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTicker>(result);

            return HttpResult.Ok(result, ParseTicker(result.Data, request.Symbol));
        }

        async Task<IExchangeCallResult<SharedTicker[]>> IGetAllTickers.GetAllTickersAsync(GetTickersRequest request, CancellationToken ct)
            => await GetAllTickersAsync(request, ct).ConfigureAwait(false);

        public async Task<HttpResult<SharedTicker[]>> GetAllTickersAsync(GetTickersRequest request, CancellationToken ct)
        {
            var validationError = GetAllTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return HttpResult.Fail<SharedTicker[]>(Exchange, validationError);

            var result = await _api.ExchangeData.GetTickersAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return HttpResult.Fail<SharedTicker[]>(result);

            return HttpResult.Ok(result, result.Data.Select(ticker => ParseTicker(ticker, ParseSymbol(ticker.Symbol))).ToArray());
        }

        private static SharedTicker ParseTicker(BitvavoTicker ticker, SharedSymbol? symbol)
            => new SharedTicker(
                symbol, ticker.Symbol, ticker.LastPrice, ticker.HighPrice, ticker.LowPrice,
                new SharedOrderQuantity(ticker.BaseQuantity, ticker.QuoteQuantity),
                ticker.OpenPrice > 0 ? (ticker.LastPrice - ticker.OpenPrice) / ticker.OpenPrice * 100 : null);
    }
}
