using Bitvavo.Net.Clients;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Testing;
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
    public class SharedSocketTests
    {
        private static readonly SharedSymbol _symbol = new SharedSymbol(TradingMode.Spot, "BTC", "EUR");

        [Test]
        public async Task MarketBuyUsesSocketOperatorOverrideAndQuoteQuantity()
        {
            using var client = new BitvavoSocketClient(options =>
            {
                options.ApiCredentials = new BitvavoCredentials("123", "456");
                options.OperatorId = 456;
            });
            using var ct = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var socket = TestHelpers.ConfigureSocketClient(client, "wss://ws.bitvavo.com/v2");
            var requests = Channel.CreateUnbounded<string>();
            socket.OnMessageSend += message => requests.Writer.TryWrite(message);

            var parameters = new ExchangeParameters();
            parameters.AddValue("Bitvavo", "operatorId", 123L);
            var pending = client.ExchangeApi.SharedApi.PlaceSpotOrderAsync(
                new PlaceSpotOrderRequest(_symbol, SharedOrderSide.Buy, SharedOrderType.Market,
                    SharedQuantity.Quote(25m), exchangeParameters: parameters), ct.Token);

            using var authentication = JsonDocument.Parse(await requests.Reader.ReadAsync(ct.Token));
            Assert.That(authentication.RootElement.GetProperty("action").GetString(), Is.EqualTo("authenticate"));
            socket.InvokeMessage("""{"event":"authenticate"}""");

            using var message = JsonDocument.Parse(await requests.Reader.ReadAsync(ct.Token));
            var request = message.RootElement;
            Assert.Multiple(() =>
            {
                Assert.That(request.GetProperty("action").GetString(), Is.EqualTo("privateCreateOrder"));
                Assert.That(request.GetProperty("market").GetString(), Is.EqualTo("BTC-EUR"));
                Assert.That(request.GetProperty("amountQuote").GetString(), Is.EqualTo("25"));
                Assert.That(request.TryGetProperty("amount", out _), Is.False);
                Assert.That(request.GetProperty("operatorId").GetInt64(), Is.EqualTo(123L));
            });

            var requestId = request.GetProperty("requestId").GetInt32();
            socket.InvokeMessage(JsonSerializer.Serialize(new
            {
                action = "privateCreateOrder",
                requestId,
                response = new { orderId = "fixture-order-id" }
            }));
            var result = await pending.WaitAsync(ct.Token);
            Assert.That(result.Success, Is.True, result.Error?.Message);
            Assert.That(result.Data!.Id, Is.EqualTo("fixture-order-id"));
        }

        [Test]
        public async Task AllTickersSubscribesToListedSymbolsAndPreservesBatches()
        {
            using var client = new BitvavoSocketClient();
            using var ct = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var socket = TestHelpers.ConfigureSocketClient(client, "wss://ws.bitvavo.com/v2");
            var requests = Channel.CreateUnbounded<string>();
            socket.OnMessageSend += message => requests.Writer.TryWrite(message);
            var tickers = new TaskCompletionSource<SharedTicker[]>(TaskCreationOptions.RunContinuationsAsynchronously);

            var pending = client.ExchangeApi.SharedApi.SubscribeToAllTickersUpdatesAsync(new SubscribeAllTickersRequest(TradingMode.Spot),
                update => tickers.TrySetResult(update.Data), ct.Token);

            using var markets = JsonDocument.Parse(await requests.Reader.ReadAsync(ct.Token));
            Assert.That(markets.RootElement.GetProperty("action").GetString(), Is.EqualTo("getMarkets"));
            var marketsId = markets.RootElement.GetProperty("requestId").GetInt32();
            socket.InvokeMessage($$"""{"action":"getMarkets","requestId":{{marketsId}},"response":[{"market":"BTC-EUR"},{"market":"ETH-EUR"}]}""");

            using var subscription = JsonDocument.Parse(await requests.Reader.ReadAsync(ct.Token));
            var request = subscription.RootElement;
            var channel = request.GetProperty("channels")[0];
            Assert.That(channel.GetProperty("name").GetString(), Is.EqualTo("ticker24h"));
            Assert.That(channel.GetProperty("markets").Deserialize<string[]>(), Is.EquivalentTo(new[] { "BTC-EUR", "ETH-EUR" }));
            var subscriptionId = request.GetProperty("requestId").GetInt32();
            socket.InvokeMessage($$"""{"event":"subscribed","requestId":{{subscriptionId}}}""");
            Assert.That((await pending.WaitAsync(ct.Token)).Success, Is.True);

            socket.InvokeMessage("""{"event":"ticker24h","data":[{"market":"BTC-EUR","last":"55000"},{"market":"XRP-EUR","last":"1"},{"market":"ETH-EUR","last":"4000"}]}""");
            var result = await tickers.Task.WaitAsync(ct.Token);
            Assert.That(result, Has.Length.EqualTo(2));
            Assert.That(result[0].Symbol, Is.EqualTo("BTC-EUR"));
            Assert.That(result[1].Symbol, Is.EqualTo("ETH-EUR"));
        }

        [Test]
        public async Task BookTickerCombinesOneSidedUpdates()
        {
            using var client = new BitvavoSocketClient();
            using var ct = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var socket = TestHelpers.ConfigureSocketClient(client, "wss://ws.bitvavo.com/v2");
            var requests = Channel.CreateUnbounded<string>();
            socket.OnMessageSend += message => requests.Writer.TryWrite(message);
            var quotes = new List<SharedBookTicker>();

            var pending = client.ExchangeApi.SharedApi.SubscribeToBookTickerUpdatesAsync(new SubscribeBookTickerRequest(_symbol),
                update => quotes.Add(update.Data), ct.Token);
            using var subscription = JsonDocument.Parse(await requests.Reader.ReadAsync(ct.Token));
            var requestId = subscription.RootElement.GetProperty("requestId").GetInt32();
            socket.InvokeMessage($$"""{"event":"subscribed","requestId":{{requestId}}}""");
            Assert.That((await pending.WaitAsync(ct.Token)).Success, Is.True);

            socket.InvokeMessage("""{"event":"ticker","market":"BTC-EUR","bestBid":"55000","bestBidSize":"2"}""");
            Assert.That(quotes, Is.Empty);
            socket.InvokeMessage("""{"event":"ticker","market":"BTC-EUR","lastPrice":"55001"}""");
            Assert.That(quotes, Is.Empty);
            socket.InvokeMessage("""{"event":"ticker","market":"BTC-EUR","bestAsk":"55002","bestAskSize":"3"}""");

            socket.InvokeMessage("""{"event":"ticker","market":"BTC-EUR","lastPrice":"55001"}""");

            Assert.That(quotes, Has.Count.EqualTo(1));
            Assert.That(quotes[0].BestBidPrice, Is.EqualTo(55000m));
            Assert.That(quotes[0].BestAskPrice, Is.EqualTo(55002m));
            Assert.That(quotes[0].BestAskQuantities.QuantityInBaseAsset, Is.EqualTo(3m));
        }
    }
}
