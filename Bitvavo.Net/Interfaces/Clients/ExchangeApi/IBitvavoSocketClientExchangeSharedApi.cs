using CryptoExchange.Net.SharedApis;

namespace Bitvavo.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// Shared interface for Exchange socket API usage
    /// </summary>
    public interface IBitvavoSocketClientExchangeSharedApi :
        IPlaceSpotOrderSocket,
        IEditSpotOrderSocket,
        IEditSpotOrderByClientOrderIdSocket,
        ICancelSpotOrderSocket,
        ICancelSpotOrderByClientOrderIdSocket,
        ICancelAllSpotOrdersSocket,
        ICancelAllSpotSymbolOrdersSocket,
        ISubscribeTickerSocket,
        ISubscribeAllTickersSocket,
        ISubscribeBookTickerSocket,
        ISubscribeKlinesSocket,
        ISubscribeIncrementalOrderBookSocket,
        ISubscribeTradesSocket,
        ISubscribeSpotOrdersSocket,
        ISubscribeUserTradesSocket
    {
    }
}
