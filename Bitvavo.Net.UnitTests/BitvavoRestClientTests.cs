using System;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using NUnit.Framework;
using System.Net.Http;
using System.Net.Http.Headers;
using Bitvavo.Net.Clients;
using Bitvavo.Net.Objects.Options;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System.Net;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Bitvavo.Net.UnitTests
{
    [TestFixture()]
    public class BitvavoRestClientTests
    {
        [TestCase(false)]
        [TestCase(true)]
        public void CheckAccountSignature(bool withQuery)
        {
            const string key = "key";
            const string secret = "bitvavo";
            var authProvider = new BitvavoAuthenticationProvider(new BitvavoCredentials(key, secret));
            using var restClient = new BitvavoRestClient();
            var client = (RestApiClient)restClient.ExchangeApi;
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            if (withQuery)
            {
                parameters.Add("quote", "EUR");
                parameters.Add("market", "BTC-EUR");
            }

            var endpoint = withQuery ? "/v2/account/fees" : "/v2/account";
            var definition = new RequestDefinition("https://api.bitvavo.com", endpoint, HttpMethod.Get) { Authenticated = true };
            var request = new RestRequestConfiguration(definition, parameters, null, null, HttpMethodParameterPosition.InUri, RequestBodyFormat.Json);

            authProvider.ProcessRequest(client, request);

            var headers = request.Headers!;
            var expectedPath = withQuery ? "/v2/account/fees?market=BTC-EUR&quote=EUR" : "/v2/account";
            var payload = headers["Bitvavo-Access-Timestamp"] + "GET" + expectedPath;
            var expectedSignature = Convert.ToHexString(HMACSHA256.HashData(Encoding.UTF8.GetBytes(secret), Encoding.UTF8.GetBytes(payload))).ToLowerInvariant();

            Assert.That(headers["Bitvavo-Access-Key"], Is.EqualTo(key));
            Assert.That(headers["Bitvavo-Access-Signature"], Is.EqualTo(expectedSignature));
        }

        [Test]
        public void CheckAtomicCancelSignature()
        {
            const string secret = "bitvavo";
            const string path = "/v2/atomic/orders";
            const string expectedBody = "{\"market\":\"BTC-EUR\",\"operatorId\":543462,\"side\":\"sell\"}";
            using var client = new BitvavoRestClient();
            var authProvider = new BitvavoAuthenticationProvider(new BitvavoCredentials("key", secret));
            var definition = new RequestDefinition("https://api.bitvavo.com", path, HttpMethod.Delete) { Authenticated = true };
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("side", "sell");
            parameters.Add("market", "BTC-EUR");
            parameters.Add("operatorId", 543462L);
            var request = new RestRequestConfiguration(definition, null, parameters, null, HttpMethodParameterPosition.InBody, RequestBodyFormat.Json);

            authProvider.ProcessRequest((RestApiClient)client.ExchangeApi, request);

            var headers = request.Headers!;
            var payload = headers["Bitvavo-Access-Timestamp"] + "DELETE" + path + expectedBody;
            var signature = Convert.ToHexString(HMACSHA256.HashData(Encoding.UTF8.GetBytes(secret), Encoding.UTF8.GetBytes(payload))).ToLowerInvariant();
            Assert.That(request.GetBodyContent(), Is.EqualTo(expectedBody));
            Assert.That(headers["Bitvavo-Access-Signature"], Is.EqualTo(signature));
        }

        [Test]
        public async Task AccountRequestUsesServerTimeAndReturnsBitvavoError()
        {
            var handler = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(request => request.RequestUri.AbsolutePath == "/v2/time" && !request.Headers.Contains("Bitvavo-Access-Signature")),
                ItExpr.IsAny<CancellationToken>())
                .Returns(() => Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent($"{{\"time\":{DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()}}}")
                }));
            handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync",
                ItExpr.Is<HttpRequestMessage>(request => request.RequestUri.AbsolutePath == "/v2/balance" && request.Headers.Contains("Bitvavo-Access-Signature")),
                ItExpr.IsAny<CancellationToken>())
                .Returns(() => Task.FromResult(new HttpResponseMessage(HttpStatusCode.Forbidden)
                {
                    Content = new StringContent("{\"errorCode\":309,\"error\":\"Invalid signature.\"}")
                }));
            using var httpClient = new HttpClient(handler.Object, disposeHandler: false);
            using var client = new BitvavoRestClient(httpClient, null, Options.Create(new BitvavoRestOptions
            {
                ApiCredentials = new BitvavoCredentials("key", "bitvavo"),
                TimestampRecalculationInterval = TimeSpan.Zero
            }));

            var result = await client.ExchangeApi.Account.GetBalancesAsync();

            handler.Protected().Verify("SendAsync", Times.AtLeastOnce(),
                ItExpr.Is<HttpRequestMessage>(request => request.RequestUri.AbsolutePath == "/v2/time"), ItExpr.IsAny<CancellationToken>());
            Assert.That(result.Success, Is.False);
            Assert.That(result.Error?.Code, Is.EqualTo(309));
            Assert.That(result.Error?.Message, Is.EqualTo("Invalid signature."));
            Assert.That(result.Error?.ErrorType, Is.EqualTo(ErrorType.Unauthorized));
        }

        [TestCase(HttpStatusCode.BadRequest, """{"errorCode":429,"error":"Too many decimals"}""", 429, ErrorType.InvalidParameter, "Too many decimals")]
        [TestCase(HttpStatusCode.NotFound, """{"errorCode":240}""", 240, ErrorType.UnknownOrder, null)]
        [TestCase(HttpStatusCode.BadRequest, """{"errorCode":999,"error":"New Bitvavo error"}""", 999, ErrorType.Unknown, "New Bitvavo error")]
        [TestCase(HttpStatusCode.BadRequest, """{"errorCode":null,"error":"Invalid request"}""", null, ErrorType.Unknown, "Invalid request")]
        [TestCase(HttpStatusCode.BadRequest, """{"errorCode":205,"error":{}}""", 205, ErrorType.InvalidParameter, null)]
        [TestCase(HttpStatusCode.BadRequest, """[]""", null, ErrorType.Unknown, null)]
        [TestCase(HttpStatusCode.ServiceUnavailable, "<html>Service unavailable</html>", null, ErrorType.DeserializationFailed, null)]
        public async Task RestErrorResponsePreservesBitvavoDetails(
            HttpStatusCode status, string body, int? code, ErrorType errorType, string message)
        {
            using var httpClient = CreateErrorResponseClient(status, body);
            using var client = new BitvavoRestClient(httpClient, null, Options.Create(new BitvavoRestOptions { RateLimiterEnabled = false }));
            var result = await client.ExchangeApi.ExchangeData.GetSymbolsAsync();

            Assert.That(result.Success, Is.False);
            Assert.That(result.Error?.ErrorCode, Is.EqualTo(code?.ToString()));
            Assert.That(result.Error?.ErrorType, Is.EqualTo(errorType));
            if (errorType != ErrorType.DeserializationFailed)
                Assert.That(result.Error?.Message, Is.EqualTo(message));
        }

        [TestCase("1735689600123", true, 1735689600123L)]
        [TestCase("999999999999999", true, 1735689600000L)]
        [TestCase(null, false, null)]
        public async Task RestRateLimitErrorUsesBitvavoResetOrRetryAfter(string reset, bool includeRetryAfter, long? expected)
        {
            using var httpClient = CreateErrorResponseClient(HttpStatusCode.TooManyRequests,
                """{"errorCode":105,"error":"Request weight limit exceeded"}""", response =>
                {
                    if (reset != null)
                        response.Headers.Add("bitvavo-ratelimit-resetat", reset);
                    if (includeRetryAfter)
                        response.Headers.RetryAfter = new RetryConditionHeaderValue(DateTimeOffset.FromUnixTimeMilliseconds(1735689600000L));
                });
            using var client = new BitvavoRestClient(httpClient, null, Options.Create(new BitvavoRestOptions { RateLimiterEnabled = false }));

            var result = await client.ExchangeApi.ExchangeData.GetSymbolsAsync();

            Assert.That(result.Error, Is.TypeOf<ServerRateLimitError>());
            var error = (ServerRateLimitError)result.Error!;
            Assert.That(error.Code, Is.EqualTo(105));
            Assert.That(error.Message, Is.EqualTo("Request weight limit exceeded"));
            Assert.That(error.RetryAfter, Is.EqualTo(expected.HasValue ? DateTimeOffset.FromUnixTimeMilliseconds(expected.Value).UtcDateTime : (DateTime?)null));
        }

        private static HttpClient CreateErrorResponseClient(HttpStatusCode status, string body, Action<HttpResponseMessage> configureResponse = null)
        {
            var handler = new Mock<HttpMessageHandler>();
            handler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .Returns(() =>
                {
                    var response = new HttpResponseMessage(status) { Content = new StringContent(body) };
                    configureResponse?.Invoke(response);
                    return Task.FromResult(response);
                });
            return new HttpClient(handler.Object);
        }

        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<BitvavoRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<BitvavoSocketClient>();
        }

        [Test]
        public void TestRestSpotSharedApiDiscoveryMatchesAggregate()
        {
            using var client = new BitvavoRestClient();
            var (missingOptions, missingInterfaces) = CryptoExchange.Net.Testing.TestHelpers.ValidateSharedApi(client.ExchangeApi.SharedApi);

            Assert.That(missingOptions, Is.Empty);
            Assert.That(missingInterfaces, Is.Empty);
        }

        [Test]
        public void TestSocketSpotSharedApiDiscoveryMatchesAggregate()
        {
            using var client = new BitvavoSocketClient();
            var (missingOptions, missingInterfaces) = CryptoExchange.Net.Testing.TestHelpers.ValidateSharedApi(client.ExchangeApi.SharedApi);

            Assert.That(missingOptions, Is.Empty);
            Assert.That(missingInterfaces, Is.Empty);
        }

        [Test]
        public void TestSpotRestSharedApiDoesntHaveUnsupportedCapabilities()
        {
            using var client = new BitvavoRestClient();
            var unsupported = CryptoExchange.Net.Testing.TestHelpers.ValidateUnsupportedCapabilities(client.ExchangeApi.SharedApi);

            Assert.That(unsupported, Is.Empty);
        }

        [Test]
        public void TestSpotSocketSharedApiDoesntHaveUnsupportedCapabilities()
        {
            using var client = new BitvavoSocketClient();
            var unsupported = CryptoExchange.Net.Testing.TestHelpers.ValidateUnsupportedCapabilities(client.ExchangeApi.SharedApi);

            Assert.That(unsupported, Is.Empty);
        }
    }
}
