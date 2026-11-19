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
        public SubscribeUserTradeOptions SubscribeUserTradeOptions { get; } = new SubscribeUserTradeOptions(_exchangeName, true);

        public Task<WebSocketResult<UpdateSubscription>> SubscribeToUserTradeUpdatesAsync(
            SubscribeUserTradeRequest request, Action<DataEvent<SharedUserTrade[]>> handler, CancellationToken ct)
        {
            var validationError = SubscribeUserTradeOptions.ValidateRequest(request, this);
            if (validationError != null)
                return Task.FromResult(WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError));

            return _api.Trading.SubscribeToUserDataUpdatesAsync(null, update =>
            {
                var trade = update.Data;
                handler(update.ToType<SharedUserTrade[]>([new SharedUserTrade(
                    ParseSymbol(trade.Symbol), trade.Symbol, trade.OrderId, trade.Id,
                    trade.Side == OrderSide.Buy ? SharedOrderSide.Buy : SharedOrderSide.Sell,
                    new SharedOrderQuantity(trade.Quantity), trade.Price, trade.TimestampPrecise ?? trade.Timestamp)
                {
                    ClientOrderId = trade.ClientOrderId,
                    Fee = trade.FeeQuantity,
                    FeeAsset = trade.FeeAsset,
                    Role = trade.IsTaker ? SharedRole.Taker : SharedRole.Maker
                }]));
            }, ct);
        }
    }
}
