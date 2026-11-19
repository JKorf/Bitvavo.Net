using CryptoExchange.Net.SharedApis;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    internal partial class BitvavoRestClientExchangeSharedApi :
        SharedApiBase,
        IBitvavoRestClientExchangeSharedApi
    {
        private readonly BitvavoRestClientExchangeApi _api;

        private const string _topicId = "BitvavoExchange";
        private const string _exchangeName = "Bitvavo";

        public override SharedClientInfo Discover() => SharedUtils.GetClientInfo(BitvavoExchange.Metadata, this);

        public BitvavoRestClientExchangeSharedApi(BitvavoRestClientExchangeApi api)
            : base(
                  SharedTransport.Rest,
                  api,
                  new[] { TradingMode.Spot },
                  () => api.Authenticated,
                  api.FormatSymbol)
        {
            _api = api;

            SetCapabilities(
                GetAssetOptions,
                GetAllAssetsOptions,
                GetBalancesOptions,
                GetDepositAddressesOptions,
                GetDepositHistoryOptions,
                GetFeeOptions,
                GetKlinesOptions,
                GetOrderBookOptions,
                GetBookTickerOptions,
                GetSpotSymbolsOptions,
                GetTickerOptions,
                GetAllTickersOptions,
                GetRecentTradesOptions,
                GetTradeHistoryOptions,
                WithdrawOptions,
                GetWithdrawalHistoryOptions,
                GetLedgerOptions,
                PlaceSpotOrderOptions,
                GetSpotOrderOptions,
                GetSpotOrderByClientOrderIdOptions,
                EditSpotOrderOptions,
                EditSpotOrderByClientOrderIdOptions,
                GetOpenSpotOrdersOptions,
                GetClosedSpotOrdersOptions,
                CancelSpotOrderOptions,
                CancelSpotOrderByClientOrderIdOptions,
                CancelAllSpotOrdersOptions,
                CancelAllSpotSymbolOrdersOptions,
                PlaceSpotTriggerOrderOptions,
                GetSpotTriggerOrderOptions,
                CancelSpotTriggerOrderOptions,
                GetSpotUserTradeHistoryOptions,
                GetSpotOrderTradesOptions);
        }
    }
}
