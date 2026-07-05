using System.Globalization;
using System.Text.Json;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net;

/// <summary>
/// Bitvavo HMAC-SHA256 authentication provider
/// </summary>
internal sealed class BitvavoAuthenticationProvider : AuthenticationProvider<BitvavoCredentials, HMACCredential>
{
    private readonly int _receiveWindowMs;
    private readonly IMessageSerializer _serializer;

    public BitvavoAuthenticationProvider(BitvavoCredentials credentials, int receiveWindowMs = 10_000)
        : base(credentials, credentials)
    {
        _receiveWindowMs = receiveWindowMs;
        _serializer = new SystemTextJsonMessageSerializer(new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }

    /// <inheritdoc />
    public override void ProcessRequest(RestApiClient apiClient, RestRequestConfiguration request)
    {
        if (!request.RequestDefinition.Authenticated) return;

        var timestamp = GetMillisecondTimestamp(apiClient, false);
        var method = request.RequestDefinition.Method.Method.ToUpperInvariant();
        var path = "/" + request.RequestDefinition.Path.TrimStart('/');

        var body = string.Empty;
        if ((request.RequestDefinition.Method == HttpMethod.Post || request.RequestDefinition.Method == HttpMethod.Put)
            && request.BodyParameters is { Count: > 0 } bodyParams)
        {
            body = GetSerializedBody(_serializer, bodyParams);
            request.SetBodyContent(body);
        }

        var query = request.GetQueryString(urlEncode: true);
        var url = string.IsNullOrEmpty(query) ? path : path + "?" + query;

        var payload = timestamp + method + url + body;
        var signature = SignHMACSHA256(payload, SignOutputType.Hex)!.ToLowerInvariant();

        var headers = request.Headers!;
        headers["Bitvavo-Access-Key"] = Key!;
        headers["Bitvavo-Access-Signature"] = signature;
        headers["Bitvavo-Access-Timestamp"] = timestamp;
        headers["Bitvavo-Access-Window"] = _receiveWindowMs.ToString(CultureInfo.InvariantCulture);
    }

    /// <summary>
    /// Build the JSON payload for Bitvavo's WebSocket authentication
    public Dictionary<string, object> BuildSocketAuth(int? receiveWindowMs = null)
    {
        var window = receiveWindowMs ?? _receiveWindowMs;

        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString(CultureInfo.InvariantCulture);
        var signature = SignHMACSHA256(timestamp + "GET/v2/websocket", SignOutputType.Hex)!.ToLowerInvariant();

        return new Dictionary<string, object>
        {
            ["action"] = "authenticate",
            ["key"] = Key!,
            ["signature"] = signature,
            ["timestamp"] = long.Parse(timestamp, CultureInfo.InvariantCulture),
            ["window"] = window,
        };
    }
}
