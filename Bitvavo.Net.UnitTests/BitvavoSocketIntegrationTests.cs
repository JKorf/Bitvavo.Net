using CryptoExchange.Net.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Bitvavo.Net.Clients;
using Bitvavo.Net.Objects.Options;

namespace Bitvavo.Net.UnitTests
{
    [NonParallelizable]
    internal class BitvavoSocketIntegrationTests : SocketIntegrationTest<BitvavoSocketClient>
    {
        public override bool Run { get; set; } = false;

        public BitvavoSocketIntegrationTests()
        {
        }

        public override BitvavoSocketClient GetClient(ILoggerFactory loggerFactory)
        {
            var key = Environment.GetEnvironmentVariable("APIKEY");
            var sec = Environment.GetEnvironmentVariable("APISECRET");

            Authenticated = key != null && sec != null;
            return new BitvavoSocketClient(Options.Create(new BitvavoSocketOptions
            {
                OutputOriginalData = true,
                ApiCredentials = Authenticated ? new BitvavoCredentials(key, sec) : null
            }), loggerFactory);
        }

        [TestCase]
        public async Task TestSubscriptions()
        {
            //await RunAndCheckUpdate<>((client, updateHandler) => client.SpotApi.Account.SubscribeToUserDataUpdatesAsync(default, default, default, default, default, default, default, default), false, true);
            //await RunAndCheckUpdate<>((client, updateHandler) => client.SpotApi.ExchangeData.SubscribeToTickerUpdatesAsync("ETHUSDT", updateHandler, default), true, false);
        } 
    }
}
