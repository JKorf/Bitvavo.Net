using CryptoExchange.Net.Interfaces.Clients;

namespace Bitvavo.Net.Interfaces.Clients.SpotApi;

/// <summary>
/// Bitvavo Spot REST API client surface
/// </summary>
public interface IBitvavoRestClientSpotApi : IRestApiClient
{
    /// <summary>
    /// Public-data endpoints (markets, candles, ticker, orderbook).
    /// </summary>
    IBitvavoRestClientSpotApiExchangeData ExchangeData { get; }

    /// <summary>
    /// Account-data endpoints (account info, balances, fees, transaction history).
    /// </summary>
    IBitvavoRestClientSpotApiAccount Account { get; }

    /// <summary>
    /// Trading endpoints (place / update / cancel orders + own-trade history).
    /// </summary>
    IBitvavoRestClientSpotApiTrading Trading { get; }

    /// <summary>
    /// Funding endpoints (deposit address, deposit / withdrawal history).
    /// </summary>
    IBitvavoRestClientSpotApiFunding Funding { get; }

    /// <summary>
    /// MiCA regulatory-reporting endpoints (trade report, order-book report).
    /// </summary>
    IBitvavoRestClientSpotApiReport Report { get; }

    /// <summary>
    /// Institutional endpoints (subaccount management, asset transfers, per-subaccount queries).
    /// </summary>
    IBitvavoRestClientSpotApiInstitutional Institutional { get; }

    /// <summary>
    /// Get the shared rest requests client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
    /// </summary>
    IBitvavoRestClientSpotApiShared SharedClient { get; }
}
