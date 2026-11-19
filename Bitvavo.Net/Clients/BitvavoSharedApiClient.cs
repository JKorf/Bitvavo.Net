using CryptoExchange.Net.Clients;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using Bitvavo.Net.Interfaces.Clients;
using Bitvavo.Net.Objects.Options;

namespace Bitvavo.Net.Clients
{
    /// <inheritdoc cref="IBitvavoSharedApiClient" />
    public class BitvavoSharedApiClient : SharedApiClientBase, IBitvavoSharedApiClient
    {
        /// <summary>
        /// ctor
        /// </summary>
        public BitvavoSharedApiClient(
            IBitvavoRestClient restClient,
            IBitvavoSocketClient socketClient,
            IOptions<BitvavoOptions> options)
            : base(options.Value.SharedApi.PreferredTransport
                #warning TODO

                )
        {
#warning TODO
        }
    }
}
