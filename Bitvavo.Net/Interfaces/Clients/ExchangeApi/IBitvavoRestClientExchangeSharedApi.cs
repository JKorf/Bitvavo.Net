using CryptoExchange.Net.SharedApis;

namespace Bitvavo.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// Shared interface for Exchange rest API usage
    /// </summary>
    public interface IBitvavoRestClientExchangeSharedApi :
        IGetAssetRest,
        IGetAllAssetsRest,
        IGetBalancesRest,
        IGetDepositAddressesRest,
        IGetDepositHistoryRest,
        IGetFeesRest,
        IGetKlinesRest,
        IGetOrderBookRest,
        IGetBookTickerRest,
        IGetSpotSymbolsRest,
        IGetTickerRest,
        IGetAllTickersRest,
        IGetRecentTradesRest,
        IGetTradeHistoryRest,
        IWithdrawRest,
        IGetWithdrawalHistoryRest,
        IGetLedgerRest,
        IPlaceSpotOrderRest,
        IGetSpotOrderRest,
        IGetSpotOrderByClientOrderIdRest,
        IEditSpotOrderRest,
        IEditSpotOrderByClientOrderIdRest,
        IGetOpenSpotOrdersRest,
        IGetClosedSpotOrdersRest,
        ICancelSpotOrderRest,
        ICancelSpotOrderByClientOrderIdRest,
        ICancelAllSpotOrdersRest,
        ICancelAllSpotSymbolOrdersRest,
        IPlaceSpotTriggerOrderRest,
        IGetSpotTriggerOrderRest,
        ICancelSpotTriggerOrderRest,
        IGetSpotUserTradeHistoryRest,
        IGetSpotOrderTradesRest
    {
    }
}
