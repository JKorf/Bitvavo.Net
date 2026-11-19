
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    internal partial class BitvavoSocketClientExchangeSharedApi
    {
        public SubscribeIncrementalOrderBookOptions SubscribeIncrementalOrderBookOptions { get; } =
            new SubscribeIncrementalOrderBookOptions(_exchangeName, false, [], SharedOrderBookSubscriptionType.OnlyIncremental)
            {
                SupportsMultipleSymbols = true,
                ParameterRuleOverrides = [RequestParameterRuleOverride<SubscribeOrderBookRequest>.NotSupported(x => x.Limit)]
            };

        public Task<WebSocketResult<UpdateSubscription>> SubscribeToIncrementalOrderBookUpdatesAsync(
            SubscribeOrderBookRequest request, Action<DataEvent<SharedIncrementalOrderBook>> handler, CancellationToken ct)
        {
            var validationError = SubscribeIncrementalOrderBookOptions.ValidateRequest(request, this);
            if (validationError != null)
                return Task.FromResult(WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError));

            return _api.ExchangeData.SubscribeToOrderBookUpdatesAsync(request.SymbolNames(FormatSymbol),
                update => handler(update.ToType(new SharedIncrementalOrderBook(
                    SharedQuantityType.BaseAsset, update.Data.Nonce, update.Data.Nonce, update.Data.Asks, update.Data.Bids))), ct);
        }
    }
}
