using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Testing;
using NUnit.Framework;
using System;
using System.Threading.Tasks;
using Bitvavo.Net.Clients;
using Bitvavo.Net.Enums;

namespace Bitvavo.Net.UnitTests
{
    [TestFixture]
    public class RestRequestTests
    {
        [Test]
        public async Task ValidateExchangeDataCalls()
        {
            using var client = new BitvavoRestClient(opts => opts.AutoTimestamp = false);
            var tester = new RestRequestValidator<BitvavoRestClient>(client, "Endpoints/Spot/ExchangeData", "https://api.bitvavo.com", IsAuthenticated);

            var startTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var endTime = startTime.AddHours(1);
            const string fromId = "00000000-0000-0431-0000-0000039c5ec6";
            const string toId = "00000000-0000-0431-0000-0000039c5ec7";

            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetOrderBookAsync("BTC-EUR", depth: 1), "GetOrderBook");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetOrderBookReportAsync("BTC-EUR", depth: 1), "GetOrderBookReport");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetOrderBookReportAsync("BTC-EUR"), "GetOrderBookReportDocumented");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetTradesAsync("BTC-EUR", fromId, toId, startTime, endTime, limit: 2), "GetTrades");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetTradesReportAsync("BTC-EUR", fromId, toId, startTime, endTime, limit: 2), "GetTradesReport");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetKlinesAsync("BTC-EUR", KlineInterval.OneHour, startTime, endTime, limit: 1), "GetKlines");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetTickerPriceAsync("BTC-EUR"), "GetTickerPrice");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetTickerPricesAsync(), "GetTickerPrices");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetBookTickerAsync("BTC-EUR"), "GetBookTicker");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetBookTickersAsync(), "GetBookTickers");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetTickerAsync("BTC-EUR"), "GetTicker");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetTickersAsync(), "GetTickers");
        }

        private bool IsAuthenticated(IHttpResult result)
        {
            return result.RequestHeaders?.Contains("Bitvavo-Access-Signature") == true;
        }
    }
}
