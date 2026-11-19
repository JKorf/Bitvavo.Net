using CryptoExchange.Net.SharedApis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    internal partial class BitvavoSocketClientExchangeSharedApi :
        SharedApiBase,
        IBitvavoSocketClientExchangeSharedApi
    {
        private readonly BitvavoSocketClientExchangeApi _api;

        private const string _topicId = "BitvavoExchange";
        private const string _exchangeName = "Bitvavo";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(BitvavoExchange.Metadata, this);

        public BitvavoSocketClientExchangeSharedApi(BitvavoSocketClientExchangeApi api)
            : base(
                  SharedTransport.Socket,
                  api,
                  new[] { TradingMode.Spot },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                PlaceSpotOrderOptions,
                EditSpotOrderOptions,
                EditSpotOrderByClientOrderIdOptions,
                CancelSpotOrderOptions,
                CancelSpotOrderByClientOrderIdOptions,
                CancelAllSpotOrdersOptions,
                CancelAllSpotSymbolOrdersOptions,
                SubscribeTickerOptions,
                SubscribeAllTickersOptions,
                SubscribeBookTickerOptions,
                SubscribeKlineOptions,
                SubscribeIncrementalOrderBookOptions,
                SubscribeTradeOptions,
                SubscribeSpotOrderOptions,
                SubscribeUserTradeOptions);
        }

        public Task UnsubscribeAllAsync() => _api.UnsubscribeAllAsync();

        private Dictionary<string, SharedSymbol> GetSubscriptionSymbols(SharedSymbolRequest request)
        {
            var symbols = new Dictionary<string, SharedSymbol>(StringComparer.Ordinal);
            foreach (var symbol in request.Symbols ?? [request.Symbol!])
                symbols[symbol.GetSymbol(FormatSymbol)] = symbol;

            return symbols;
        }

        private SharedSymbol? ParseSymbol(string symbol)
            => BitvavoSharedApiMappings.ParseSymbol(_topicId, _api.EnvironmentName, symbol);
    }
}
