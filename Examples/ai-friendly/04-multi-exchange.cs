// 04-multi-exchange.cs
//
// Demonstrates resolving the Shared API through dependency injection and using
// an exchange-independent REST ticker capability.
//
// Install with dotnet add package JKorf.Bitvavo.Net

using Bitvavo.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();
services.AddBitvavo();
using var provider = services.BuildServiceProvider();
var bitvavo = provider.GetRequiredService<IBitvavoSharedApiClient>();

var symbol = new SharedSymbol(TradingMode.Spot, "BTC", "EUR");
// Use ExchangeRest for Shared REST calls and ExchangeSocket for Shared WebSocket operations.
await PrintTickerAsync(bitvavo.ExchangeRest, symbol);

// The method also accepts any other exchange's IGetTickerRest implementation.
static async Task PrintTickerAsync(IGetTickerRest tickerClient, SharedSymbol symbol)
{
    var result = await tickerClient.GetTickerAsync(new GetTickerRequest(symbol));
    if (!result.Success)
    {
        Console.Error.WriteLine($"Shared ticker request failed: {result.Error}");
        return;
    }

    Console.WriteLine($"{result.Data.Symbol} last={result.Data.LastPrice} 24h change={result.Data.ChangePercentage}%");
}
