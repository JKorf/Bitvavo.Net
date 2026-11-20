using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net;
using CryptoExchange.Net.SharedApis;

namespace Bitvavo.Net.Utils
{
    internal static class BitvavoSharedApiMappings
    {
        internal static SharedOrderType ParseOrderType(BitvavoOrder order)
            => order.Type switch
            {
                OrderType.Market or OrderType.StopLoss or OrderType.TakeProfit => SharedOrderType.Market,
                OrderType.Limit when order.PostOnly == true => SharedOrderType.LimitMaker,
                OrderType.Limit or OrderType.StopLossLimit or OrderType.TakeProfitLimit => SharedOrderType.Limit,
                _ => SharedOrderType.Other
            };

        internal static SharedOrderStatus ParseOrderStatus(OrderStatus? status)
            => status switch
            {
                OrderStatus.New or OrderStatus.AwaitingTrigger or OrderStatus.PartiallyFilled => SharedOrderStatus.Open,
                OrderStatus.Canceled or OrderStatus.Expired => SharedOrderStatus.Canceled,
                OrderStatus.Filled => SharedOrderStatus.Filled,
                _ => SharedOrderStatus.Unknown
            };

        internal static TimeInForce? MapTimeInForce(SharedTimeInForce? timeInForce)
            => timeInForce switch
            {
                SharedTimeInForce.GoodTillCanceled => TimeInForce.GoodTillCanceled,
                SharedTimeInForce.ImmediateOrCancel => TimeInForce.ImmediateOrCancel,
                SharedTimeInForce.FillOrKill => TimeInForce.FillOrKill,
                _ => null
            };

        internal static SharedTimeInForce? ParseTimeInForce(TimeInForce? timeInForce)
            => timeInForce switch
            {
                TimeInForce.GoodTillCanceled => SharedTimeInForce.GoodTillCanceled,
                TimeInForce.ImmediateOrCancel => SharedTimeInForce.ImmediateOrCancel,
                TimeInForce.FillOrKill => SharedTimeInForce.FillOrKill,
                _ => null
            };

        internal static decimal? GetAveragePrice(BitvavoOrder order)
            => order.FilledQuantity > 0 ? order.FilledQuoteQuantity / order.FilledQuantity : null;

        internal static SharedTicker ParseTicker(BitvavoTicker ticker, SharedSymbol? symbol)
            => new SharedTicker(
                symbol, ticker.Symbol, ticker.LastPrice, ticker.HighPrice, ticker.LowPrice,
                new SharedOrderQuantity(ticker.BaseQuantity, ticker.QuoteQuantity),
                ticker.OpenPrice > 0 ? (ticker.LastPrice - ticker.OpenPrice) / ticker.OpenPrice * 100 : null);

        internal static SharedSymbol? ParseSymbol(string topicId, string environmentName, string symbol)
        {
            var cached = ExchangeSymbolCache.ParseSymbol(topicId, environmentName, null, symbol);
            if (cached != null)
                return cached;

            var separator = symbol.IndexOf('-');
            if (separator <= 0 || separator == symbol.Length - 1)
                return null;

            return new SharedSymbol(TradingMode.Spot, symbol.Substring(0, separator), symbol.Substring(separator + 1)) { SymbolName = symbol };
        }

        internal static ExchangeParameterDescription OperatorIdRule
            => ExchangeParameterRule.Optional("operatorId", "Override the operator ID configured in the Bitvavo client options", 1L);

        internal static long? GetOperatorId(SharedRequest request, string exchangeName)
            => request.GetParamValue<long?>(exchangeName, "operatorId");
    }
}
