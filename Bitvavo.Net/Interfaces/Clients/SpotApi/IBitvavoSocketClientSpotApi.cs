using CryptoExchange.Net.Interfaces.Clients;

namespace Bitvavo.Net.Interfaces.Clients.SpotApi;

/// <summary>
/// Bitvavo Spot WebSocket API client surface
/// </summary>
public interface IBitvavoSocketClientSpotApi : ISocketApiClient
{
    /// <summary>
    /// Public-stream subscriptions (candles, trades, ticker, book).
    /// </summary>
    IBitvavoSocketClientSpotApiExchangeData ExchangeData { get; }

    /// <summary>
    /// Private-stream subscriptions on the <c>account</c> channel
    /// </summary>
    IBitvavoSocketClientSpotApiAccount Account { get; }

    /// <summary>
    /// Get the shared socket client. This interface is shared with other exchanges to allow for a common implementation for different exchanges.
    /// </summary>
    IBitvavoSocketClientSpotApiShared SharedClient { get; }
}
