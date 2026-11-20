using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Bitvavo.Net.Utils.BitvavoSharedApiMappings;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    internal partial class BitvavoSocketClientExchangeSharedApi
    {
        public SubscribeTickerOptions SubscribeTickerOptions { get; } = new SubscribeTickerOptions(_exchangeName, SharedTickerType.Day24H)
        {
            SupportsMultipleSymbols = true
        };

        public SubscribeTickersOptions SubscribeAllTickersOptions { get; } = new SubscribeTickersOptions(_exchangeName, SharedTickerType.Day24H);

        public SubscribeBookTickerOptions SubscribeBookTickerOptions { get; } = new SubscribeBookTickerOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true
        };

        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTickerUpdatesAsync(
            SubscribeTickerRequest request, Action<DataEvent<SharedTicker>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return Task.FromResult(WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError));

            var symbols = GetSubscriptionSymbols(request);
            return _api.ExchangeData.SubscribeToTicker24hUpdatesAsync(symbols.Keys,
                update => handler(update.ToType(ParseTicker(update.Data, symbols[update.Data.Symbol]))), ct);
        }

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToAllTickersUpdatesAsync(
            SubscribeAllTickersRequest request, Action<DataEvent<SharedTicker[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeAllTickersOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var result = await _api.ExchangeData.GetSymbolsAsync(ct).ConfigureAwait(false);
            if (!result.Success)
                return WebSocketResult.Fail<UpdateSubscription>(result);

            return await _api.ExchangeData.SubscribeToTicker24hBatchUpdatesAsync(result.Data.Select(symbol => symbol.Symbol),
                update => handler(update.ToType(update.Data.Select(ticker => ParseTicker(ticker, ParseSymbol(ticker.Symbol))).ToArray())),
                ct).ConfigureAwait(false);
        }

        public async Task<WebSocketResult<UpdateSubscription>> SubscribeToBookTickerUpdatesAsync(
            SubscribeBookTickerRequest request, Action<DataEvent<SharedBookTicker>> handler, CancellationToken ct)
        {
            var validationError = SubscribeBookTickerOptions.ValidateRequest(request, this);
            if (validationError != null)
                return WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError);

            var symbols = GetSubscriptionSymbols(request);
            var quotes = new ConcurrentDictionary<string, BitvavoTickerUpdate>(StringComparer.Ordinal);
            var result = await _api.ExchangeData.SubscribeToTickerUpdatesAsync(symbols.Keys, update =>
            {
                var ticker = update.Data;
                if (!ticker.BestAskPrice.HasValue && !ticker.BestAskQuantity.HasValue
                    && !ticker.BestBidPrice.HasValue && !ticker.BestBidQuantity.HasValue)
                    return;

                // A ticker event can contain only the side that changed.
                var quote = quotes.AddOrUpdate(ticker.Symbol, ticker, (_, previous) => ticker with
                {
                    BestAskPrice = ticker.BestAskPrice ?? previous.BestAskPrice,
                    BestAskQuantity = ticker.BestAskPrice.HasValue ? ticker.BestAskQuantity : ticker.BestAskQuantity ?? previous.BestAskQuantity,
                    BestBidPrice = ticker.BestBidPrice ?? previous.BestBidPrice,
                    BestBidQuantity = ticker.BestBidPrice.HasValue ? ticker.BestBidQuantity : ticker.BestBidQuantity ?? previous.BestBidQuantity
                });

                if (quote.BestAskPrice.HasValue && quote.BestBidPrice.HasValue)
                    handler(update.ToType(new SharedBookTicker(
                        symbols[ticker.Symbol], ticker.Symbol, quote.BestAskPrice.Value, new SharedOrderQuantity(quote.BestAskQuantity),
                        quote.BestBidPrice.Value, new SharedOrderQuantity(quote.BestBidQuantity))));
            }, ct).ConfigureAwait(false);

            if (result.Success)
                result.Data.ConnectionLost += quotes.Clear;

            return result;
        }
    }
}
