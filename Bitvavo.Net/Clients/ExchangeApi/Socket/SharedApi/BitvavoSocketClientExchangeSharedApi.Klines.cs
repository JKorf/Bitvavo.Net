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
        public SubscribeKlineOptions SubscribeKlineOptions { get; } = new SubscribeKlineOptions(_exchangeName, false,
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
            SharedKlineInterval.OneDay)
        {
            SupportsMultipleSymbols = true
        };

        public Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(
            SubscribeKlineRequest request, Action<DataEvent<SharedKline>> handler, CancellationToken ct)
        {
            var validationError = SubscribeKlineOptions.ValidateRequest(request, this);
            if (validationError != null)
                return Task.FromResult(WebSocketResult.Fail<UpdateSubscription>(Exchange, validationError));

            var symbols = GetSubscriptionSymbols(request);
            return _api.ExchangeData.SubscribeToKlineUpdatesAsync(symbols.Keys, (KlineInterval)request.Interval, update =>
            {
                var symbol = update.Data.Symbol;
                foreach (var kline in update.Data.Klines)
                    handler(update.ToType(new SharedKline(
                        symbols[symbol], symbol, kline.OpenTime, kline.ClosePrice, kline.HighPrice, kline.LowPrice, kline.OpenPrice,
                        new SharedOrderQuantity(kline.Quantity))));
            }, ct);
        }
    }
}
