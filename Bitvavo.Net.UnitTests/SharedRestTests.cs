using Bitvavo.Net.Clients;
using Bitvavo.Net.Objects.Options;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Options;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Bitvavo.Net.UnitTests
{
    [TestFixture]
    public class SharedRestTests
    {
        private static readonly SharedSymbol _symbol = new SharedSymbol(TradingMode.Spot, "BTC", "EUR");

        [TestCase(false)]
        [TestCase(true)]
        public async Task MarketBuySendsQuoteQuantityAndOperatorId(bool overrideOperatorId)
        {
            using var handler = new FixtureHandler("""{"orderId":"fixture-order-id","market":"BTC-EUR"}""");
            using var httpClient = new HttpClient(handler);
            using var client = CreateClient(httpClient);
            var parameters = new ExchangeParameters();
            if (overrideOperatorId)
                parameters.AddValue("Bitvavo", "operatorId", 123L);

            var result = await client.ExchangeApi.SharedApi.PlaceSpotOrderAsync(
                new PlaceSpotOrderRequest(_symbol, SharedOrderSide.Buy, SharedOrderType.Market,
                    SharedQuantity.Quote(25m), exchangeParameters: parameters));

            Assert.That(result.Success, Is.True, result.Error?.Message);
            Assert.That(result.Data!.Id, Is.EqualTo("fixture-order-id"));
            using var request = JsonDocument.Parse(handler.RequestBody!);
            Assert.That(request.RootElement.GetProperty("amountQuote").GetString(), Is.EqualTo("25"));
            Assert.That(request.RootElement.TryGetProperty("amount", out _), Is.False);
            Assert.That(request.RootElement.GetProperty("operatorId").GetInt64(), Is.EqualTo(overrideOperatorId ? 123L : 456L));
        }

        [Test]
        public async Task TradeHistoryUsesTradeIdToForTheNextPage()
        {
            using var handler = new FixtureHandler(
                """[{"id":"3","timestamp":1735772400000,"amount":"0.1","price":"95000","side":"buy"},{"id":"2","timestamp":1735772400000,"amount":"0.2","price":"95000","side":"sell"}]""",
                """[{"id":"1","timestamp":1735772400000,"amount":"0.3","price":"95000","side":"buy"}]""");
            using var httpClient = new HttpClient(handler);
            using var client = CreateClient(httpClient);
            var endTime = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc);
            var request = new GetTradeHistoryRequest(_symbol, endTime.AddHours(-2), endTime, limit: 2);

            var first = await client.ExchangeApi.SharedApi.GetTradeHistoryAsync(request);
            Assert.That(first.Success, Is.True, first.Error?.Message);
            Assert.That(first.Data, Has.Length.EqualTo(2));
            Assert.That(first.NextPageRequest?.FromId, Is.EqualTo("2"));

            var next = await client.ExchangeApi.SharedApi.GetTradeHistoryAsync(request, first.NextPageRequest);
            Assert.That(next.Success, Is.True, next.Error?.Message);
            Assert.That(next.Data, Has.Length.EqualTo(1));
            Assert.That(next.NextPageRequest, Is.Null);
            Assert.That(handler.RequestUri!.Query, Does.Contain("tradeIdTo=2"));
            Assert.That(handler.RequestUri.Query, Does.Contain("end=1735776000000"));
        }

        [Test]
        public async Task ClosedOrderHistoryPaginatesFromTheUnfilteredPage()
        {
            using var handler = new FixtureHandler(
                """
                [
                  {"orderId":"closed-order","market":"BTC-EUR","created":1735772400000,"status":"filled","side":"buy","orderType":"limit","filledAmount":"0.1","filledAmountQuote":"9500"},
                  {"orderId":"open-order","market":"BTC-EUR","created":1735772399000,"status":"new","side":"buy","orderType":"limit"}
                ]
                """);
            using var httpClient = new HttpClient(handler);
            using var client = CreateClient(httpClient);
            var endTime = new DateTime(2025, 1, 2, 0, 0, 0, DateTimeKind.Utc);
            var result = await client.ExchangeApi.SharedApi.GetClosedSpotOrdersAsync(
                new GetClosedOrdersRequest(_symbol, endTime: endTime, limit: 2));

            Assert.That(result.Success, Is.True, result.Error?.Message);
            Assert.That(result.Data, Has.Length.EqualTo(1));
            Assert.That(result.Data![0].OrderId, Is.EqualTo("closed-order"));
            Assert.That(result.Data[0].AveragePrice, Is.EqualTo(95000m));
            Assert.That(result.NextPageRequest?.FromId, Is.EqualTo("open-order"));
        }

        [Test]
        public async Task LedgerPaginatesWhenOneTransactionProducesMultipleEntries()
        {
            const string fixture = """
                {"items":[{"transactionId":"transaction-id","executedAt":"2025-01-01T23:00:00Z","type":"buy","sentCurrency":"EUR","sentAmount":"100","receivedCurrency":"BTC","receivedAmount":"0.001","feesCurrency":"EUR","feesAmount":"0.25"}],"currentPage":1,"totalPages":1,"maxItems":1}
                """;
            using var handler = new FixtureHandler(fixture, fixture, fixture);
            using var httpClient = new HttpClient(handler);
            using var client = CreateClient(httpClient);
            var request = new GetLedgerRequest(limit: 1);

            var first = await client.ExchangeApi.SharedApi.GetLedgerAsync(request);
            Assert.That(first.Success, Is.True, first.Error?.Message);
            Assert.That(first.Data, Has.Length.EqualTo(1));
            Assert.That(first.Data![0].DeltaQuantity, Is.EqualTo(-100m));
            Assert.That(first.NextPageRequest?.Offset, Is.EqualTo(1));

            var second = await client.ExchangeApi.SharedApi.GetLedgerAsync(request, first.NextPageRequest);
            Assert.That(second.Success, Is.True, second.Error?.Message);
            Assert.That(second.Data, Has.Length.EqualTo(1));
            Assert.That(second.Data![0].DeltaQuantity, Is.EqualTo(0.001m));
            Assert.That(second.NextPageRequest?.Offset, Is.EqualTo(2));

            var last = await client.ExchangeApi.SharedApi.GetLedgerAsync(request, second.NextPageRequest);
            Assert.That(last.Success, Is.True, last.Error?.Message);
            Assert.That(last.Data, Has.Length.EqualTo(1));
            Assert.That(last.Data![0].DeltaQuantity, Is.EqualTo(-0.25m));
            Assert.That(last.NextPageRequest, Is.Null);
        }

        private static BitvavoRestClient CreateClient(HttpClient httpClient)
            => new BitvavoRestClient(httpClient, null, Options.Create(new BitvavoRestOptions
            {
                ApiCredentials = new BitvavoCredentials("123", "456"),
                AutoTimestamp = false,
                OperatorId = 456L,
                RateLimiterEnabled = false
            }));

        private sealed class FixtureHandler : HttpMessageHandler
        {
            private readonly Queue<string> _responses;

            public Uri? RequestUri { get; private set; }
            public string? RequestBody { get; private set; }

            public FixtureHandler(params string[] responses) => _responses = new Queue<string>(responses);

            protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
            {
                RequestUri = request.RequestUri;
                RequestBody = request.Content == null ? null : await request.Content.ReadAsStringAsync(cancellationToken).ConfigureAwait(false);
                return new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new StringContent(_responses.Dequeue(), Encoding.UTF8, "application/json")
                };
            }
        }
    }
}
