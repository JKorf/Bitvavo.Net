using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using System.Collections.Generic;
using System.Net.Http;

namespace Bitvavo.Net
{
    internal class BitvavoAuthenticationProvider : AuthenticationProvider<BitvavoCredentials, BitvavoCredentials>
    {
        private static readonly SystemTextJsonMessageSerializer _serializer = new SystemTextJsonMessageSerializer(BitvavoExchange._serializerContext);

        public BitvavoAuthenticationProvider(BitvavoCredentials credentials) : base(credentials, credentials)
        {
        }


        public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration requestConfig)
        {
            if (!requestConfig.RequestDefinition.Authenticated)
                return;

            var timestamp = GetMillisecondTimestamp(apiClient);
            var query = requestConfig.GetQueryString();
            var path = string.IsNullOrEmpty(query) ? requestConfig.RequestDefinition.Path : $"{requestConfig.RequestDefinition.Path}?{query}";
            var body = string.Empty;

            if (requestConfig.RequestDefinition.Method != HttpMethod.Get && requestConfig.ParameterPosition == HttpMethodParameterPosition.InBody)
            {
                body = requestConfig.GetBodyContent() ?? (requestConfig.BodyParameters is { Empty: false } parameters
                    ? GetSerializedBody(_serializer, parameters)
                    : string.Empty);
                requestConfig.SetBodyContent(body);
            }

            var signature = SignHMACSHA256(timestamp + requestConfig.RequestDefinition.Method.Method + path + body, SignOutputType.Hex).ToLowerInvariant();
            requestConfig.Headers ??= new Dictionary<string, string>();
            requestConfig.Headers["Bitvavo-Access-Key"] = Credential.Key;
            requestConfig.Headers["Bitvavo-Access-Timestamp"] = timestamp;
            requestConfig.Headers["Bitvavo-Access-Signature"] = signature;
        }

        public override Query? GetAuthenticationQuery(SocketApiClient apiClient, SocketConnection connection, Dictionary<string, object?>? context = null)
        {
            return null;
        }
    }
}
