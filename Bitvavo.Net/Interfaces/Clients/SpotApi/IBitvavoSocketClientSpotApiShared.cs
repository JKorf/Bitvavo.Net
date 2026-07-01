using CryptoExchange.Net.SharedApis;

namespace Bitvavo.Net.Interfaces.Clients.SpotApi;

/// <summary>
/// Shared interface for Spot socket API usage
/// </summary>
public interface IBitvavoSocketClientSpotApiShared :
    IKlineSocketClient,
    ITradeSocketClient,
    ISpotOrderSocketClient,
    IUserTradeSocketClient
{
}
