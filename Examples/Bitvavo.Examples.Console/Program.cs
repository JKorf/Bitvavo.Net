using Bitvavo.Net.Clients;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Bitvavo.Net.Objects.Options;
using Microsoft.Extensions.Options;

// REST
var restClient = new BitvavoRestClient();
var ticker = await restClient.ExchangeApi.ExchangeData.GetTickerAsync("ETH-EUR");
if (!ticker.Success)
{
    Console.WriteLine($"Failed to get ticker: {ticker.Error}");
    return;
}

Console.WriteLine($"Rest client ticker price for ETH-EUR: {ticker.Data.LastPrice}");

Console.WriteLine();
Console.WriteLine("Press enter to start websocket subscription");
Console.ReadLine();

// Websocket
// Optional, manually add logging
var logFactory = new LoggerFactory();
logFactory.AddProvider(new TraceLoggerProvider());

var socketClient = new BitvavoSocketClient(Options.Create(new BitvavoSocketOptions { }), logFactory);
var subscription = await socketClient.ExchangeApi.ExchangeData.SubscribeToTickerUpdatesAsync("ETH-EUR", update =>
{
    if (update.Data.LastPrice != null)
        Console.WriteLine($"Websocket client ticker price for ETH-EUR: {update.Data.LastPrice}");
});

if (!subscription.Success)
{
    Console.WriteLine($"Failed to subscribe to ticker updates: {subscription.Error}");
    return;
}

Console.ReadLine();
