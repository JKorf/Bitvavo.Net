using Bitvavo.Net.Interfaces.Clients;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using Bitvavo.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Options;

namespace Bitvavo.Net.Clients
{
    /// <inheritdoc cref="IBitvavoSharedApiClient" />
    public class BitvavoSharedApiClient : SharedApiClientBase, IBitvavoSharedApiClient
    {
        /// <inheritdoc />
        public IBitvavoRestClientExchangeSharedApi ExchangeRest { get; }

        /// <summary>
        /// ctor
        /// </summary>
        public BitvavoSharedApiClient(
            IBitvavoRestClient restClient,
            IBitvavoSocketClient socketClient,
            IOptions<BitvavoOptions> options)
            : base(options.Value.SharedApi.PreferredTransport, restClient.ExchangeApi.SharedApi)
        {
            ExchangeRest = restClient.ExchangeApi.SharedApi;
        }
    }
}
