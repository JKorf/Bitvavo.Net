using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System.Threading.Tasks;
using Bitvavo.Net.Clients;
using Bitvavo.Net.Objects.Models;
using Bitvavo.Net.Objects.Options;

namespace Bitvavo.Net.UnitTests
{
    [TestFixture]
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateSubscriptions()
        {
            var client = new BitvavoSocketClient(opts =>
            {
                opts.ApiCredentials = new BitvavoCredentials("123", "456");
            });
            var tester = new SocketSubscriptionValidator<BitvavoSocketClient>(client, "Subscriptions/Spot", "XXX");
            //await tester.ValidateAsync<BitvavoModel>((client, handler) => client.SpotApi.SubscribeToXXXUpdatesAsync(handler), "XXX");
        }

        [TestCase]
        public async Task ValidateConcurrentSpotSubscriptions()
        {
            var logger = new LoggerFactory();
            logger.AddProvider(new TraceLoggerProvider());

            var client = new BitvavoSocketClient(Options.Create(new BitvavoSocketOptions
            {
                ApiCredentials = new BitvavoCredentials("123", "456"),
                OutputOriginalData = true
            }), logger);

            var tester = new SocketSubscriptionValidator<BitvavoSocketClient>(client, "Subscriptions/Spot", "XXX", "data");
            //await tester.ValidateConcurrentAsync<BitvavoModel>(
            //    (client, handler) => client.SpotApi.ExchangeData.SubscribeToKlineUpdatesAsync("BTCUSDT", Enums.KlineInterval.EightHour, handler),
            //    (client, handler) => client.SpotApi.ExchangeData.SubscribeToKlineUpdatesAsync("BTCUSDT", Enums.KlineInterval.OneHour, handler),
            //    "Concurrent");
        }
    }
}
