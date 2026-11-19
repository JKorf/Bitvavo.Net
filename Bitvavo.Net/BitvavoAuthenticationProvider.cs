using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using System.Collections.Generic;

namespace Bitvavo.Net
{
    internal class BitvavoAuthenticationProvider : AuthenticationProvider<BitvavoCredentials, BitvavoCredentials>
    {
#warning TODO

        public BitvavoAuthenticationProvider(BitvavoCredentials credentials) : base(credentials, credentials)
        {
        }


        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration requestConfig)
        {
            if (!requestConfig.RequestDefinition.Authenticated)
                return;

#warning TODO
        }

        public override Query? GetAuthenticationQuery(SocketApiClient apiClient, SocketConnection connection, Dictionary<string, object?>? context = null)
        {
            return null;
        }
    }
}
