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

            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetSymbolAsync("BTC-EUR"), "GetSymbol");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetSymbolsAsync(), "GetSymbols");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetAssetAsync("BTC"), "GetAsset");
            await tester.ValidateAsync(client => client.ExchangeApi.ExchangeData.GetAssetsAsync(), "GetAssets");
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

        [Test]
        public async Task ValidateAccountCalls()
        {
            using var client = new BitvavoRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.ApiCredentials = new BitvavoCredentials("123", "456");
            });
            var tester = new RestRequestValidator<BitvavoRestClient>(client, "Endpoints/Spot/Account", "https://api.bitvavo.com", IsAuthenticated);

            var startTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var endTime = startTime.AddDays(1);
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetAccountFeesAsync(), "GetAccountFees", nestedJsonProperty: "fees");
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetBalancesAsync("BTC"), "GetBalances");
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetStakingBalancesAsync("BTC"), "GetStakingBalances");
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetSymbolFeesAsync("BTC-EUR"), "GetSymbolFees");
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetSymbolFeesAsync(quoteAsset: FeeQuoteAsset.Usdc), "GetQuoteAssetFees");
            await tester.ValidateAsync(client => client.ExchangeApi.Account.GetTransactionHistoryAsync(TransactionType.FixedStaking, startTime, endTime, page: 2, pageSize: 5), "GetTransactionHistory");
        }

        [Test]
        public async Task ValidateTradingCalls()
        {
            const long defaultOperatorId = 543462;
            const long overrideOperatorId = 765432;
            using var client = new BitvavoRestClient(opts =>
            {
                opts.AutoTimestamp = false;
                opts.RateLimiterEnabled = false;
                opts.OperatorId = defaultOperatorId;
                opts.ApiCredentials = new BitvavoCredentials("123", "456");
            });
            var tester = new RestRequestValidator<BitvavoRestClient>(client, "Endpoints/Spot/Trading", "https://api.bitvavo.com", IsAuthenticated);

            const string orderId = "1be6d0df-d5dc-4b53-a250-3376f3b393e6";
            const string clientOrderId = "2be7d0df-d8dc-7b93-a550-8876f3b393e9";
            const string tradeId = "108c3633-0276-4480-a902-17a01829deae";
            var startTime = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc);
            var endTime = startTime.AddHours(1);

            await tester.ValidateAsync(client => client.ExchangeApi.Trading.PlaceOrderAsync("BTC-EUR", OrderSide.Sell, OrderType.Limit, clientOrderId, quantity: 0.01m, price: 56000m, timeInForce: TimeInForce.GoodTillCanceled, postOnly: true, selfTradePrevention: SelfTradePrevention.CancelOldest, cancelGroupId: 2), "PlaceOrder");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.UpdateOrderAsync("BTC-EUR", clientOrderId: clientOrderId, remainingQuantity: 0.02m, price: 57000m, responseRequired: false), "UpdateOrder");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetOrderAsync("BTC-EUR", orderId), "GetOrder");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.CancelOrderAsync("BTC-EUR", clientOrderId: clientOrderId), "CancelOrder");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetOpenOrdersAsync("BTC-EUR"), "GetOpenOrders");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetOpenOrdersAsync(baseAsset: "BTC"), "GetOpenOrdersByBaseAsset");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetUserTradesAsync("BTC-EUR", fromId: tradeId, toId: tradeId, startTime: startTime, endTime: endTime, limit: 3), "GetUserTrades");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.GetOrderHistoryAsync("BTC-EUR", orderId, orderId, startTime, endTime, limit: 3), "GetOrderHistory");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.CancelOrdersAsync("BTC-EUR"), "CancelOrders");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.CancelOrdersAsync(), "CancelAllOrders");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.CancelOrdersAtomicAsync("BTC-EUR", OrderSide.Sell, operatorId: overrideOperatorId), "CancelOrdersAtomic");
            await tester.ValidateAsync(client => client.ExchangeApi.Trading.CancelOrdersAfterAsync(2, 30), "CancelOrdersAfter");
        }

        private bool IsAuthenticated(IHttpResult result)
        {
            return result.RequestHeaders?.Contains("Bitvavo-Access-Signature") == true;
        }
    }
}
