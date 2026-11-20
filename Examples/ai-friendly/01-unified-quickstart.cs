// 01-unified-quickstart.cs
//
// Demonstrates public spot market data and an authenticated balance query.
//
// Install with dotnet add package JKorf.Bitvavo.Net

using Bitvavo.Net;
using Bitvavo.Net.Clients;
using Bitvavo.Net.Enums;

// Bitvavo symbols use the BASE-QUOTE format.
const string symbol = "BTC-EUR";
var apiKey = Environment.GetEnvironmentVariable("BITVAVO_API_KEY");
var apiSecret = Environment.GetEnvironmentVariable("BITVAVO_API_SECRET");

// Public ExchangeData calls work without credentials. Add credentials for Account calls.
using var client = new BitvavoRestClient(options =>
{
    if (!string.IsNullOrWhiteSpace(apiKey) && !string.IsNullOrWhiteSpace(apiSecret))
        options.ApiCredentials = new BitvavoCredentials(apiKey, apiSecret);
});

var exchange = client.ExchangeApi;
// The symbol details include the tick size and order quantity limits.
var market = await exchange.ExchangeData.GetSymbolAsync(symbol);
if (!market.Success)
{
    Console.Error.WriteLine($"Could not load {symbol}: {market.Error}");
    return;
}

Console.WriteLine($"{market.Data.Symbol} is {market.Data.Status}. Tick size: {market.Data.TickSize}. Minimum quantity: {market.Data.MinOrderQuantity} {market.Data.BaseAsset}");

var ticker = await exchange.ExchangeData.GetTickerAsync(symbol);
if (!ticker.Success)
{
    Console.Error.WriteLine($"Could not load ticker: {ticker.Error}");
    return;
}

Console.WriteLine($"{symbol} last price: {ticker.Data.LastPrice} {market.Data.QuoteAsset}");

var candles = await exchange.ExchangeData.GetKlinesAsync(symbol, KlineInterval.OneHour, limit: 3);
if (candles.Success)
{
    foreach (var candle in candles.Data.OrderBy(x => x.OpenTime))
        Console.WriteLine($"{candle.OpenTime:u} open={candle.OpenPrice} close={candle.ClosePrice} quantity={candle.Quantity}");
}
else
{
    Console.Error.WriteLine($"Could not load candles: {candles.Error}");
}

if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(apiSecret))
    return;

// Filter balances by asset code. The quote asset for BTC-EUR is EUR.
var balances = await exchange.Account.GetBalancesAsync(asset: market.Data.QuoteAsset);
if (!balances.Success)
{
    Console.Error.WriteLine($"Could not load balances: {balances.Error}");
    return;
}

foreach (var balance in balances.Data)
    Console.WriteLine($"{balance.Asset} available={balance.AvailableQuantity} in orders={balance.InOrderQuantity}");
