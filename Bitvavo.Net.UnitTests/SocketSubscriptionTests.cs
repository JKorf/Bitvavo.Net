using Bitvavo.Net.Clients;
using Bitvavo.Net.Enums;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
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
    public class SocketSubscriptionTests
    {
        [Test]
        public async Task ValidateSubscriptions()
        {
            using var client = new BitvavoSocketClient();
            var tester = new SocketSubscriptionValidator<BitvavoSocketClient>(client, "Subscriptions/Spot", "wss://ws.bitvavo.com/v2");
            var ignored = new List<string> { "event" };

            await tester.ValidateAsync<BitvavoTickerUpdate>((c, handler) => c.ExchangeApi.ExchangeData.SubscribeToTickerUpdatesAsync("BTC-EUR", handler), "Ticker", ignoreProperties: ignored);
            await tester.ValidateAsync<BitvavoTicker>((c, handler) => c.ExchangeApi.ExchangeData.SubscribeToTicker24hUpdatesAsync("BTC-EUR", handler), "Ticker24h", nestedJsonProperty: "data.0");
            await tester.ValidateAsync<BitvavoTicker>((c, handler) => c.ExchangeApi.ExchangeData.SubscribeToTicker24hUpdatesAsync("BTC-EUR", handler), "Ticker24hSingle", nestedJsonProperty: "data");
            await tester.ValidateAsync<BitvavoKlineUpdate>((c, handler) => c.ExchangeApi.ExchangeData.SubscribeToKlineUpdatesAsync("BTC-EUR", KlineInterval.OneMinute, handler), "Kline", ignoreProperties: ignored);
            await tester.ValidateAsync<BitvavoTradeUpdate>((c, handler) => c.ExchangeApi.ExchangeData.SubscribeToTradeUpdatesAsync("BTC-EUR", handler), "Trade", ignoreProperties: ignored);
            await tester.ValidateAsync<BitvavoOrderBook>((c, handler) => c.ExchangeApi.ExchangeData.SubscribeToOrderBookUpdatesAsync("BTC-EUR", handler), "Book", ignoreProperties: ignored);
        }

        [TestCase("ticker", false)]
        [TestCase("ticker", true)]
        [TestCase("ticker24h", false)]
        [TestCase("ticker24h", true)]
        [TestCase("candles", false)]
        [TestCase("candles", true)]
        public async Task IndependentAndDuplicateStreamsShareAConnection(string channelName, bool duplicate)
        {
            using var client = new BitvavoSocketClient();
            using var ct = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var socket = TestHelpers.ConfigureSocketClient(client, "wss://ws.bitvavo.com/v2");
            var requests = Channel.CreateUnbounded<string>();
            socket.OnMessageSend += message => requests.Writer.TryWrite(message);

            var firstUpdates = 0;
            var secondUpdates = 0;
            var secondSymbol = duplicate || channelName == "candles" ? "BTC-EUR" : "ETH-EUR";
            var secondInterval = duplicate ? KlineInterval.OneMinute : KlineInterval.FiveMinutes;
            var first = await SubscribeAsync("BTC-EUR", KlineInterval.OneMinute, () => firstUpdates++);
            var connection = socket.Connection;
            await SubscribeAsync(secondSymbol, secondInterval, () => secondUpdates++, expectRequest: !duplicate);
            Assert.That(socket.Connection, Is.SameAs(connection));

            PublishUpdates();
            Assert.That((firstUpdates, secondUpdates), Is.EqualTo((1, 1)));

            var close = first.Data!.CloseAsync();
            if (!duplicate)
            {
                using var unsubscribe = await AcknowledgeAsync("unsubscribed");
                var unsubscribedChannel = unsubscribe.RootElement.GetProperty("channels")[0];
                Assert.That(unsubscribedChannel.GetProperty("name").GetString(), Is.EqualTo(channelName));
                Assert.That(unsubscribedChannel.GetProperty("markets").Deserialize<string[]>(), Is.EqualTo(new[] { "BTC-EUR" }));
                if (channelName == "candles")
                    Assert.That(unsubscribedChannel.GetProperty("interval").Deserialize<string[]>(), Is.EqualTo(new[] { "1m" }));
            }
            await close.WaitAsync(ct.Token);
            Assert.That(requests.Reader.TryRead(out _), Is.False);

            PublishUpdates();
            Assert.That((firstUpdates, secondUpdates), Is.EqualTo((1, 2)));

            async Task<WebSocketResult<UpdateSubscription>> SubscribeAsync(string symbol, KlineInterval interval, Action onUpdate, bool expectRequest = true)
            {
                var exchangeData = client.ExchangeApi.ExchangeData;
                var subscription = channelName switch
                {
                    "ticker" => exchangeData.SubscribeToTickerUpdatesAsync(symbol, _ => onUpdate(), ct.Token),
                    "ticker24h" => exchangeData.SubscribeToTicker24hUpdatesAsync(symbol, _ => onUpdate(), ct.Token),
                    _ => exchangeData.SubscribeToKlineUpdatesAsync(symbol, interval, _ => onUpdate(), ct.Token)
                };
                if (expectRequest)
                {
                    using var request = await AcknowledgeAsync("subscribed");
                    Assert.That(request.RootElement.GetProperty("action").GetString(), Is.EqualTo("subscribe"));
                }
                Assert.That(requests.Reader.TryRead(out _), Is.False);
                var result = await subscription.WaitAsync(ct.Token);
                Assert.That(result.Success, Is.True);
                return result;
            }

            async Task<JsonDocument> AcknowledgeAsync(string eventName)
            {
                var request = JsonDocument.Parse(await requests.Reader.ReadAsync(ct.Token));
                var requestId = request.RootElement.GetProperty("requestId").GetInt32();
                socket.InvokeMessage(JsonSerializer.Serialize(new { @event = eventName, requestId }));
                return request;
            }

            void PublishUpdates()
            {
                switch (channelName)
                {
                    case "ticker":
                        socket.InvokeMessage("""{"event":"ticker","market":"BTC-EUR","bestBid":"10400"}""");
                        socket.InvokeMessage("""{"event":"ticker","market":"ETH-EUR","bestBid":"1200"}""");
                        break;
                    case "ticker24h":
                        socket.InvokeMessage("""{"event":"ticker24h","data":[{"market":"BTC-EUR","last":"10400"},{"market":"ETH-EUR","last":"1200"}]}""");
                        break;
                    default:
                        socket.InvokeMessage("""{"event":"candle","market":"BTC-EUR","interval":"1m","candle":[]}""");
                        socket.InvokeMessage("""{"event":"candle","market":"BTC-EUR","interval":"5m","candle":[]}""");
                        break;
                }
            }
        }

        [Test]
        public async Task UnsubscribeLeavesOtherChannelsActive()
        {
            using var client = new BitvavoSocketClient();
            using var ct = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var socket = TestHelpers.ConfigureSocketClient(client, "wss://ws.bitvavo.com/v2");
            var requests = Channel.CreateUnbounded<string>();
            socket.OnMessageSend += message => requests.Writer.TryWrite(message);

            var exchangeData = client.ExchangeApi.ExchangeData;
            var ticker = exchangeData.SubscribeToTickerUpdatesAsync(["BTC-EUR", "ETH-EUR"], _ => { }, ct.Token);
            using var tickerRequest = JsonDocument.Parse(await requests.Reader.ReadAsync(ct.Token));
            var tickerId = tickerRequest.RootElement.GetProperty("requestId").GetInt32();
            socket.InvokeMessage($$"""{"event":"subscribed","requestId":{{tickerId}}}""");
            var tickerSubscription = await ticker;
            Assert.That(tickerSubscription.Success, Is.True);

            var tradeUpdate = new TaskCompletionSource<BitvavoTradeUpdate>(TaskCreationOptions.RunContinuationsAsynchronously);
            var trades = exchangeData.SubscribeToTradeUpdatesAsync("BTC-EUR", update => tradeUpdate.TrySetResult(update.Data), ct.Token);
            using var tradesRequest = JsonDocument.Parse(await requests.Reader.ReadAsync(ct.Token));
            var tradesId = tradesRequest.RootElement.GetProperty("requestId").GetInt32();
            socket.InvokeMessage($$"""{"event":"subscribed","requestId":{{tradesId}}}""");
            Assert.That((await trades).Success, Is.True);

            var close = tickerSubscription.Data!.CloseAsync();
            using var unsubscribe = JsonDocument.Parse(await requests.Reader.ReadAsync(ct.Token));
            var unsubId = unsubscribe.RootElement.GetProperty("requestId").GetInt32();
            var channel = unsubscribe.RootElement.GetProperty("channels")[0];
            Assert.Multiple(() =>
            {
                Assert.That(unsubscribe.RootElement.GetProperty("action").GetString(), Is.EqualTo("unsubscribe"));
                Assert.That(channel.GetProperty("name").GetString(), Is.EqualTo("ticker"));
                Assert.That(channel.GetProperty("markets").Deserialize<string[]>(), Is.EquivalentTo(new[] { "BTC-EUR", "ETH-EUR" }));
                Assert.That(channel.TryGetProperty("interval", out _), Is.False);
            });
            socket.InvokeMessage($$"""{"event":"unsubscribed","requestId":{{unsubId}}}""");
            await close.WaitAsync(ct.Token);

            socket.InvokeMessage("""{"event":"trade","market":"BTC-EUR","id":"after-unsubscribe","price":"10400","amount":"0.01","side":"buy","timestamp":1735689600000}""");
            Assert.That((await tradeUpdate.Task.WaitAsync(ct.Token)).Id, Is.EqualTo("after-unsubscribe"));
        }
    }
}
