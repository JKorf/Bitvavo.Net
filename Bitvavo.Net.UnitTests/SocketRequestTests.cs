using Bitvavo.Net.Clients;
using Bitvavo.Net.Enums;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
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
            await tester.ValidateAsync(client, client => client.ExchangeApi.ExchangeData.GetServerTimeAsync(), "GetServerTime",
                responseMapper: time => new { time });
            await tester.ValidateAsync(client, client => client.ExchangeApi.ExchangeData.GetSymbolAsync("BTC-EUR"), "GetSymbol");
            await tester.ValidateAsync(client, client => client.ExchangeApi.ExchangeData.GetSymbolsAsync(), "GetSymbols");
            await tester.ValidateAsync(client, client => client.ExchangeApi.ExchangeData.GetAssetAsync("BTC"), "GetAsset");
            await tester.ValidateAsync(client, client => client.ExchangeApi.ExchangeData.GetAssetsAsync(), "GetAssets");
        }

        [Test]
        public async Task ValidateAccountCalls()
        {
            using var client = new BitvavoSocketClient(opts =>
            {
                opts.ApiCredentials = new BitvavoCredentials("123", "456");
            });
            var tester = new SocketRequestValidator<BitvavoSocketClient>("Socket/Spot/Account", "response");

            var startTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var endTime = startTime.AddDays(1);
            await tester.ValidateAsync(client, client => client.ExchangeApi.Account.GetAccountFeesAsync(), "GetAccountFees", nestedJsonProperty: "response.fees");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Account.GetBalancesAsync("BTC"), "GetBalances");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Account.GetSymbolFeesAsync("BTC-EUR"), "GetSymbolFees");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Account.GetSymbolFeesAsync(quoteAsset: FeeQuoteAsset.Usdc), "GetQuoteAssetFees");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Account.GetTransactionHistoryAsync(TransactionType.FixedStaking, startTime, endTime, page: 2, pageSize: 5), "GetTransactionHistory");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Account.GetDepositDataAsync("XRP"), "GetDepositData");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Account.GetDepositDataAsync("EUR"), "GetFiatDepositData");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Account.GetDepositHistoryAsync(startTime: startTime, endTime: endTime, limit: 2), "GetDepositHistory");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Account.WithdrawAsync("XRP", "SampleWithdrawalAddress", 12.5m, paymentId: "10002653", addWithdrawalFee: true), "Withdraw");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Account.WithdrawCryptoAsync("XRP", "XRP", "SampleWithdrawalAddress", 12.5m, memo: "10002653", deductFeeFromQuantity: true, idempotencyKey: "sample-withdrawal-1"), "WithdrawCrypto");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Account.GetWithdrawalHistoryAsync("XRP", startTime, endTime, limit: 2), "GetWithdrawalHistory");
        }

        [Test]
        public async Task ValidateTradingCalls()
        {
            const long defaultOperatorId = 543462;
            const long overrideOperatorId = 765432;
            using var client = new BitvavoSocketClient(opts =>
            {
                opts.RateLimiterEnabled = false;
                opts.OperatorId = defaultOperatorId;
                opts.ApiCredentials = new BitvavoCredentials("123", "456");
            });
            var tester = new SocketRequestValidator<BitvavoSocketClient>("Socket/Spot/Trading", "response");

            const string orderId = "1be6d0df-d5dc-4b53-a250-3376f3b393e6";
            const string clientOrderId = "2be7d0df-d8dc-7b93-a550-8876f3b393e9";
            const string tradeId = "108c3633-0276-4480-a902-17a01829deae";
            var startTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var endTime = startTime.AddHours(1);

            await tester.ValidateAsync(client, client => client.ExchangeApi.Trading.PlaceOrderAsync("BTC-EUR", OrderSide.Sell, OrderType.Limit, clientOrderId, quantity: 0.01m, price: 56000m, timeInForce: TimeInForce.GoodTillCanceled, postOnly: true, selfTradePrevention: SelfTradePrevention.CancelOldest, cancelGroupId: 2), "PlaceOrder");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Trading.UpdateOrderAsync("BTC-EUR", clientOrderId: clientOrderId, remainingQuantity: 0.02m, price: 57000m, responseRequired: false), "UpdateOrder");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Trading.GetOrderAsync("BTC-EUR", orderId), "GetOrder");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Trading.CancelOrderAsync("BTC-EUR", clientOrderId: clientOrderId), "CancelOrder");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Trading.GetOpenOrdersAsync("BTC-EUR"), "GetOpenOrders");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Trading.GetOpenOrdersAsync(baseAsset: "BTC"), "GetOpenOrdersByBaseAsset");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Trading.GetUserTradesAsync("BTC-EUR", fromId: tradeId, toId: tradeId, startTime: startTime, endTime: endTime, limit: 3), "GetUserTrades");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Trading.GetOrderHistoryAsync("BTC-EUR", orderId, orderId, startTime, endTime, limit: 3), "GetOrderHistory");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Trading.CancelOrdersAsync("BTC-EUR"), "CancelOrders");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Trading.CancelOrdersAsync(), "CancelAllOrders");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Trading.CancelOrdersAtomicAsync("BTC-EUR", OrderSide.Sell, operatorId: overrideOperatorId), "CancelOrdersAtomic");
            await tester.ValidateAsync(client, client => client.ExchangeApi.Trading.CancelOrdersAfterAsync(2, 30), "CancelOrdersAfter");
        }

        [Test]
        public async Task PrivateQueriesRequireCredentials()
        {
            using var client = new BitvavoSocketClient();

            var result = await client.ExchangeApi.Account.GetAccountFeesAsync();
            Assert.That(result.Error, Is.TypeOf<NoApiCredentialsError>());
        }

        [TestCase(true)]
        [TestCase(false)]
        public async Task PrivateQueryAuthenticatesTheConnection(bool acceptAuthentication)
        {
            using var client = new BitvavoSocketClient(opts => opts.ApiCredentials = new BitvavoCredentials("123", "456"));
            using var ct = new CancellationTokenSource(TimeSpan.FromSeconds(5));
            var socket = TestHelpers.ConfigureSocketClient(client, "wss://ws.bitvavo.com/v2");
            var requests = Channel.CreateUnbounded<string>();
            socket.OnMessageSend += message => requests.Writer.TryWrite(message);

            var pending = client.ExchangeApi.Account.GetAccountFeesAsync(ct.Token);
            using var authentication = JsonDocument.Parse(await requests.Reader.ReadAsync(ct.Token));
            var request = authentication.RootElement;
            Assert.That(request.GetProperty("action").GetString(), Is.EqualTo("authenticate"));
            Assert.That(request.GetProperty("key").GetString(), Is.EqualTo("123"));

            var timestamp = request.GetProperty("timestamp").GetInt64();
            var signedText = timestamp.ToString(CultureInfo.InvariantCulture) + "GET/v2/websocket";
            var expectedSignature = HMACSHA256.HashData(Encoding.UTF8.GetBytes("456"), Encoding.UTF8.GetBytes(signedText));
            Assert.That(request.GetProperty("signature").GetString(), Is.EqualTo(Convert.ToHexStringLower(expectedSignature)));

            if (acceptAuthentication)
            {
                socket.InvokeMessage("""{"event":"authenticate"}""");
                using var query = JsonDocument.Parse(await requests.Reader.ReadAsync(ct.Token));
                Assert.That(query.RootElement.GetProperty("action").GetString(), Is.EqualTo("privateGetAccount"));
                var id = query.RootElement.GetProperty("requestId").GetInt32();
                socket.InvokeMessage(JsonSerializer.Serialize(new { requestId = id, response = new { fees = new { taker = "0.0025" } } }));
                var result = await pending.WaitAsync(ct.Token);
                Assert.That(result.Data?.TakerFee, Is.EqualTo(0.0025m));
            }
            else
            {
                var id = request.GetProperty("requestId").GetInt32();
                socket.InvokeMessage(JsonSerializer.Serialize(new { action = "authenticate", requestId = id, errorCode = 305, error = "No active API key found." }));
                var result = await pending.WaitAsync(ct.Token);
                Assert.That(result.Error?.ErrorCode, Is.EqualTo("305"));
                Assert.That(requests.Reader.TryRead(out _), Is.False);
            }
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
