# Usage examples

Each snippet uses the imports below. Account and trading snippets that use `client` assume the credentials and operator ID from the authentication example. For private WebSocket subscriptions, use a `BitvavoSocketClient` configured with the same credentials.

## Imports

```csharp
using System;
using System.Threading.Tasks;
using Bitvavo.Net;
using Bitvavo.Net.Clients;
using Bitvavo.Net.Enums;
using Bitvavo.Net.Interfaces;
using Bitvavo.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.DependencyInjection;
```

## Public market data and results

```csharp
using var client = new BitvavoRestClient();
var ticker = await client.ExchangeApi.ExchangeData.GetTickerAsync("BTC-EUR");
if (!ticker.Success)
{
    Console.Error.WriteLine(ticker.Error);
    return;
}

Console.WriteLine(ticker.Data.LastPrice);
```

On failure, inspect `Error.ErrorType`, `Error.ErrorCode`, and `Error.Message`. WebSocket queries use `QueryResult<T>` and follow the same pattern.

## Authentication

Use the key, secret, and operator ID from your application's configuration. Balance reads require credentials. Trading operations that accept an operator ID use the configured default unless overridden.

```csharp
using var client = new BitvavoRestClient(options =>
{
    options.ApiCredentials = new BitvavoCredentials(apiKey, apiSecret);
    options.OperatorId = operatorId;
});
```

## Account balances

```csharp
var result = await client.ExchangeApi.Account.GetBalancesAsync(asset: "EUR");
if (!result.Success)
{
    Console.Error.WriteLine(result.Error);
    return;
}

foreach (var balance in result.Data)
    Console.WriteLine($"{balance.Asset} available={balance.AvailableQuantity} in orders={balance.InOrderQuantity}");
```

## Limit order and cancellation

This snippet submits a live order when run. Supply a base-asset `quantity` and quote-asset `limitPrice` that fit the chosen symbol. See [order handling](safety.md#orders-and-cancellations).

```csharp
const string symbol = "BTC-EUR";
var trading = client.ExchangeApi.Trading;
var clientOrderId = Guid.NewGuid().ToString();

var placed = await trading.PlaceOrderAsync(symbol, OrderSide.Buy, OrderType.Limit,
    quantity: quantity, price: limitPrice, clientOrderId: clientOrderId,
    timeInForce: TimeInForce.GoodTillCanceled, postOnly: true);

if (!placed.Success)
{
    Console.Error.WriteLine($"Client order ID {clientOrderId}: {placed.Error}");
    return;
}

var canceled = await trading.CancelOrderAsync(symbol, orderId: placed.Data.Id);
if (!canceled.Success)
    Console.Error.WriteLine(canceled.Error);
```

## WebSocket lifecycle

```csharp
using var socket = new BitvavoSocketClient();
var result = await socket.ExchangeApi.ExchangeData.SubscribeToTicker24hBatchUpdatesAsync(
    new[] { "BTC-EUR", "ETH-EUR" },
    update =>
    {
        foreach (var ticker in update.Data)
            Console.WriteLine($"{ticker.Symbol} last={ticker.LastPrice}");
    });

if (!result.Success)
{
    Console.Error.WriteLine(result.Error);
    return;
}

try
{
    await Task.Delay(TimeSpan.FromSeconds(30));
}
finally
{
    await result.Data.CloseAsync();
}
```

For authenticated fills, use `socket.ExchangeApi.Trading.SubscribeToUserDataUpdatesAsync` with an `onTradeMessage` handler. Add an `onOrderMessage` handler when you also need order updates.

## Shared API V2

```csharp
using var client = new BitvavoRestClient();
IGetTickerRest tickers = client.ExchangeApi.SharedApi;
var symbol = new SharedSymbol(TradingMode.Spot, "BTC", "EUR");
var result = await tickers.GetTickerAsync(new GetTickerRequest(symbol));

if (result.Success)
    Console.WriteLine(result.Data.LastPrice);
else
    Console.Error.WriteLine(result.Error);
```

## Dependency injection

```csharp
var services = new ServiceCollection();
services.AddBitvavo(options =>
{
    options.ApiCredentials = new BitvavoCredentials(apiKey, apiSecret);
    options.Rest.OperatorId = operatorId;
    options.Socket.OperatorId = operatorId;
});

using var provider = services.BuildServiceProvider();
var rest = provider.GetRequiredService<IBitvavoRestClient>();
var shared = provider.GetRequiredService<IBitvavoSharedApiClient>();
var books = provider.GetRequiredService<IBitvavoOrderBookFactory>();
```

## Complete examples in a checkout

- [Market data and balances](../../../../Examples/ai-friendly/01-unified-quickstart.cs)
- [Spot trading](../../../../Examples/ai-friendly/02-spot-trading.cs)
- [WebSockets](../../../../Examples/ai-friendly/03-websocket.cs)
- [Shared API](../../../../Examples/ai-friendly/04-multi-exchange.cs)
- [Error handling](../../../../Examples/ai-friendly/05-error-handling.cs)
