using Bitvavo.Net.Clients;
using Bitvavo.Net.Enums;
using Bitvavo.Net.Interfaces.Clients;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Testing;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Bitvavo.Net.UnitTests
{
    [TestFixture]
    public class BitvavoSharedApiRestIntegrationTest : SharedApiRestIntegrationTest
    {
        protected override bool ManualRun => false;

        [Test]
        public override async Task RunAsync() => await ExecuteAsync();

        protected override ISharedApiClientBase GetClient()
        {
            var collection = new ServiceCollection();
            collection.AddBitvavo();
            var provider = collection.BuildServiceProvider();
            return provider.GetRequiredService<IBitvavoSharedApiClient>();
        }

        protected override SharedApiTestConfig GetConfig(ISharedRest client)
        {
            return new SharedApiTestConfig(new SharedSymbol(TradingMode.Spot, "ETH", "EUR"), "ETH");
        }
    }

    [TestFixture]
    public class BitvavoSharedApiSocketIntegrationTest : SharedApiSocketIntegrationTest
    {
        protected override bool ManualRun => false;

        [Test]
        public override async Task RunAsync()
        {
            var result = await ExecuteAsync();
            foreach (var item in result)
                Assert.Warn(item.Message);
        }

        protected override ISharedApiClientBase GetClient()
        {
            var collection = new ServiceCollection();
            collection.AddBitvavo();
            var provider = collection.BuildServiceProvider();
            return provider.GetRequiredService<IBitvavoSharedApiClient>();
        }

        protected override SharedApiTestConfig GetConfig(ISharedSocket client)
        {
            return new SharedApiTestConfig(new SharedSymbol(TradingMode.Spot, "ETH", "EUR"), "ETH");
        }
    }
}
