// 03-websocket.cs
//
// Demonstrates public 24-hour ticker batches and subscription teardown.
//
// Install with dotnet add package JKorf.Bitvavo.Net

using Bitvavo.Net.Clients;

using var client = new BitvavoSocketClient();
var exchangeData = client.ExchangeApi.ExchangeData;

// This stream delivers the tickers in each message together. A batch can
// contain updates for only some of the subscribed symbols.
var subscription = await exchangeData.SubscribeToTicker24hBatchUpdatesAsync(
    new[] { "BTC-EUR", "ETH-EUR" },
    update =>
    {
        foreach (var ticker in update.Data)
            Console.WriteLine($"{ticker.Symbol} last={ticker.LastPrice} bid={ticker.BestBidPrice} ask={ticker.BestAskPrice}");
    });

if (!subscription.Success)
{
    Console.Error.WriteLine($"Could not subscribe to 24-hour tickers: {subscription.Error}");
    return;
}

Console.WriteLine("Listening for 24-hour ticker batches. Press Enter to close the subscription.");
Console.ReadLine();
// Close this subscription when the consumer stops listening.
await subscription.Data.CloseAsync();
