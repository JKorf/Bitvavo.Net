using Bitvavo.Net.Interfaces.Clients.SpotApi;
using CryptoExchange.Net.Interfaces.Clients;

namespace Bitvavo.Net.Interfaces.Clients;

/// <summary>
/// Top-level Bitvavo REST client. Mirrors <c>IKrakenRestClient</c> / <c>IBinanceRestClient</c>
/// â€” single composition root with one accessor per market segment. Bitvavo only offers
/// spot trading, so there's only <see cref="SpotApi"/>.
/// </summary>
public interface IBitvavoRestClient : IRestClient<BitvavoCredentials>
{
    /// <summary>
    /// Spot REST API endpoints.
    /// </summary>
    IBitvavoRestClientSpotApi SpotApi { get; }
}
