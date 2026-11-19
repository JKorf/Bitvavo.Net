using System;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Objects;
using NUnit.Framework;
using System.Net.Http;
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
                .Returns(() => Task.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("{\"errorCode\":302,\"error\":\"API key not found.\"}")
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
            Assert.That(result.Error?.Code, Is.EqualTo(302));
            Assert.That(result.Error?.Message, Is.EqualTo("API key not found."));
        }

        [Test]
        public void CheckInterfaces()
        {
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingRestInterfaces<BitvavoRestClient>();
            CryptoExchange.Net.Testing.TestHelpers.CheckForMissingSocketInterfaces<BitvavoSocketClient>();
        }

#warning TODO

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
