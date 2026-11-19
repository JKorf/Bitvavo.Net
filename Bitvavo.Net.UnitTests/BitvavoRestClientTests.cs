using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using NUnit.Framework;
using System.Collections.Generic;
using System.Net.Http;
using Bitvavo.Net.Clients;

namespace Bitvavo.Net.UnitTests
{
    [TestFixture()]
    public class BitvavoRestClientTests
    {
        [Test]
        public void CheckSignatureExample1()
        {
            var authProvider = new BitvavoAuthenticationProvider(new BitvavoCredentials("XXX", "XXX"));
            var client = (RestApiClient)new BitvavoRestClient().ExchangeApi;

            CryptoExchange.Net.Testing.TestHelpers.CheckSignature(
                client,
                authProvider,
                HttpMethod.Post,
                "/api/v3/order",
                (uriParams, bodyParams, headers) =>
                {
                    return bodyParams["signature"].ToString();
                },
                "c8db56825ae71d6d79447849e617115f4a920fa2acdcab2b053c4b2838bd6b71",
                new Parameters(BitvavoExchange._parameterSerializationSettings)
                {
                    { "symbol", "LTCBTC" },
                },
                DateTimeConverter.ParseFromDouble(1499827319559),
                false);
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
            //var (missingOptions, missingInterfaces) = CryptoExchange.Net.Testing.TestHelpers.ValidateSharedApi(new BitvavoRestClient().SpotApi.SharedApi);

            //Assert.That(missingOptions, Is.Empty);
            //Assert.That(missingInterfaces, Is.Empty);
        }

        [Test]
        public void TestSocketSpotSharedApiDiscoveryMatchesAggregate()
        {
            //var (missingOptions, missingInterfaces) = CryptoExchange.Net.Testing.TestHelpers.ValidateSharedApi(new BitvavoRestClient().SpotApi.SharedApi);

            //Assert.That(missingOptions, Is.Empty);
            //Assert.That(missingInterfaces, Is.Empty);
        }

        [Test]
        public void TestSpotRestSharedApiDoesntHaveUnsupportedCapabilities()
        {
            //var unsupported = CryptoExchange.Net.Testing.TestHelpers.ValidateUnsupportedCapabilities(new BitvavoRestClient().SpotApi.SharedApi);

            //Assert.That(unsupported, Is.Empty);
        }

        [Test]
        public void TestSpotSocketSharedApiDoesntHaveUnsupportedCapabilities()
        {
            //var unsupported = CryptoExchange.Net.Testing.TestHelpers.ValidateUnsupportedCapabilities(new BitvavoRestClient().SpotApi.SharedApi);

            //Assert.That(unsupported, Is.Empty);
        }
    }
}
