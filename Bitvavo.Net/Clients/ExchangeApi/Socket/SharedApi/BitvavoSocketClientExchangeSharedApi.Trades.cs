using Bitvavo.Net.Enums;

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
        public SubscribeTradeOptions SubscribeTradeOptions { get; } = new SubscribeTradeOptions(_exchangeName, false)
        {
            SupportsMultipleSymbols = true
        };

        public Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
            SubscribeTradeRequest request, Action<DataEvent<SharedTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return Task.FromResult(WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError));

            var symbols = GetSubscriptionSymbols(request);
            return _api.ExchangeData.SubscribeToTradeUpdatesAsync(symbols.Keys, update =>
            {
                var trade = update.Data;
                handler(update.ToType<SharedTrade[]>([new SharedTrade(
                    symbols[trade.Symbol], trade.Symbol, new SharedOrderQuantity(trade.Quantity), trade.Price, trade.TimestampPrecise ?? trade.Timestamp)
                {
                    Side = trade.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell
                }]));
            }, ct);
        }
    }
}
