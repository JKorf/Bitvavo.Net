using CryptoExchange.Net.SharedApis;

namespace Bitvavo.Net.Interfaces.Clients.SpotApi;

/// <summary>
/// Shared interface for Spot REST API usage
/// </summary>
public interface IBitvavoRestClientSpotApiShared :
    IAssetsRestClient,
    IKlineRestClient,
    IRecentTradeRestClient,
    IOrderBookRestClient,
    ISpotSymbolRestClient,
    ISpotTickerRestClient,
    IBookTickerRestClient,
    IBalanceRestClient,
    ISpotOrderRestClient,
    ISpotOrderClientIdRestClient,
    IFeeRestClient,
    IDepositRestClient,
    IWithdrawalRestClient,
    IWithdrawRestClient
{
}
