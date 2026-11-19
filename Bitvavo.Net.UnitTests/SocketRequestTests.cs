using Bitvavo.Net.Clients;
using Bitvavo.Net.Enums;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System;
using System.Text.Json;
using System.Threading;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace Bitvavo.Net.UnitTests
{
    [TestFixture]
    public class SocketRequestTests
    {
        [Test]
        public async Task ValidateExchangeDataCalls()
        {
            using var client = new BitvavoSocketClient();
            var tester = new SocketRequestValidator<BitvavoSocketClient>("Socket/Spot/ExchangeData", "response");

            var startTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var endTime = startTime.AddHours(1);
            const string fromId = "00000000-0000-0431-0000-0000039c5ec6";
            const string toId = "00000000-0000-0431-0000-0000039c5ec7";

            await tester.ValidateAsync(client, client => client.ExchangeApi.ExchangeData.GetOrderBookAsync("BTC-EUR", depth: 1), "GetOrderBook");
            await tester.ValidateAsync(client, client => client.ExchangeApi.ExchangeData.GetTradesAsync("BTC-EUR", fromId, toId, startTime, endTime, limit: 2), "GetTrades");
            await tester.ValidateAsync(client, client => client.ExchangeApi.ExchangeData.GetKlinesAsync("BTC-EUR", KlineInterval.OneHour, startTime, endTime, limit: 1), "GetKlines");
            await tester.ValidateAsync(client, client => client.ExchangeApi.ExchangeData.GetTickerPriceAsync("BTC-EUR"), "GetTickerPrice");
            await tester.ValidateAsync(client, client => client.ExchangeApi.ExchangeData.GetTickerPricesAsync(), "GetTickerPrices");
            await tester.ValidateAsync(client, client => client.ExchangeApi.ExchangeData.GetBookTickerAsync("BTC-EUR"), "GetBookTicker");
            await tester.ValidateAsync(client, client => client.ExchangeApi.ExchangeData.GetBookTickersAsync(), "GetBookTickers");
            await tester.ValidateAsync(client, client => client.ExchangeApi.ExchangeData.GetTickerAsync("BTC-EUR"), "GetTicker");
            await tester.ValidateAsync(client, client => client.ExchangeApi.ExchangeData.GetTickersAsync(), "GetTickers");
        }

        [Test]
        public async Task RoutesOutOfOrderResponsesForTheSameAction()
        {
            using var client = new BitvavoSocketClient();
            using var ct = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var socket = TestHelpers.ConfigureSocketClient(client, "wss://ws.bitvavo.com/v2");
            var requests = Channel.CreateUnbounded<string>();
            socket.OnMessageSend += message => requests.Writer.TryWrite(message);

            var first = client.ExchangeApi.ExchangeData.GetTickerPriceAsync("INVALID", ct.Token);
            using var firstRequest = JsonDocument.Parse(await requests.Reader.ReadAsync(ct.Token));
            var firstId = firstRequest.RootElement.GetProperty("requestId").GetInt32();

            var second = client.ExchangeApi.ExchangeData.GetTickerPriceAsync("BTC-EUR", ct.Token);
            using var secondRequest = JsonDocument.Parse(await requests.Reader.ReadAsync(ct.Token));
            var secondId = secondRequest.RootElement.GetProperty("requestId").GetInt32();

            socket.InvokeMessage($$$"""{"action":"getTickerPrice","requestId":{{{secondId}}},"response":{"market":"BTC-EUR","price":"5001.1"}}""");
            socket.InvokeMessage($$"""{"action":"getTickerPrice","requestId":{{firstId}},"errorCode":205,"error":"market parameter is invalid."}""");

            var success = await second;
            var failure = await first;
            Assert.Multiple(() =>
            {
                Assert.That(secondId, Is.Not.EqualTo(firstId));
                Assert.That(success.Success, Is.True);
                Assert.That(success.Data?.Symbol, Is.EqualTo("BTC-EUR"));
                Assert.That(success.Data?.Price, Is.EqualTo(5001.1m));
                Assert.That(failure.Error, Is.TypeOf<ServerError>());
                Assert.That(failure.Error?.ErrorCode, Is.EqualTo("205"));
                Assert.That(failure.Error?.Message, Is.EqualTo("market parameter is invalid."));
            });
        }
    }
}
