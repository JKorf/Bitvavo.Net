// 02-spot-trading.cs
//
// Demonstrates placing, querying, and canceling a live spot limit order.
// The example takes the BTC quantity and EUR limit price as command-line arguments.
//
// Install with dotnet add package JKorf.Bitvavo.Net

using System.Globalization;
using Bitvavo.Net;
using Bitvavo.Net.Clients;
using Bitvavo.Net.Enums;

const string symbol = "BTC-EUR";
var apiKey = Environment.GetEnvironmentVariable("BITVAVO_API_KEY");
var apiSecret = Environment.GetEnvironmentVariable("BITVAVO_API_SECRET");
var operatorIdValue = Environment.GetEnvironmentVariable("BITVAVO_OPERATOR_ID");

if (args.Length != 2
    || !decimal.TryParse(args[0], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var quantity) || quantity <= 0
    || !decimal.TryParse(args[1], NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var price) || price <= 0)
{
    Console.WriteLine("Usage: dotnet run -- <quantity-in-BTC> <limit-price-in-EUR>");
    return;
}

if (string.IsNullOrWhiteSpace(apiKey) || string.IsNullOrWhiteSpace(apiSecret)
    || !long.TryParse(operatorIdValue, NumberStyles.None, CultureInfo.InvariantCulture, out var operatorId) || operatorId <= 0)
{
    Console.Error.WriteLine("Set BITVAVO_API_KEY, BITVAVO_API_SECRET and BITVAVO_OPERATOR_ID.");
    return;
}

using var client = new BitvavoRestClient(options =>
{
    options.ApiCredentials = new BitvavoCredentials(apiKey, apiSecret);
    // Trading requests use this operator ID by default.
    options.OperatorId = operatorId;
});

var trading = client.ExchangeApi.Trading;
// Keep this ID so you can look up the order if the placement response is lost.
var clientOrderId = Guid.NewGuid().ToString();
Console.WriteLine($"Submitting a post-only limit buy for {quantity} {symbol} at {price}. Client order ID: {clientOrderId}");

// Quantity is in the base asset. The limit price is in the quote asset.
// Post-only rejects an order that would take liquidity immediately.
var placed = await trading.PlaceOrderAsync(symbol, OrderSide.Buy, OrderType.Limit,
    quantity: quantity, price: price, clientOrderId: clientOrderId,
    timeInForce: TimeInForce.GoodTillCanceled, postOnly: true);

if (!placed.Success)
{
    Console.Error.WriteLine($"Order request failed: {placed.Error}. Lookup client order ID {clientOrderId} if the outcome is uncertain.");
    return;
}

var orderId = placed.Data.Id;
Console.WriteLine($"Placed order {orderId}.");

// Use the exchange-assigned order ID to look up or cancel the order.
var order = await trading.GetOrderAsync(symbol, orderId: orderId);
if (order.Success)
    Console.WriteLine($"Order {orderId} is {order.Data.Status}. Filled quantity: {order.Data.FilledQuantity}");
else
    Console.Error.WriteLine($"Could not look up {orderId}: {order.Error}");

var canceled = await trading.CancelOrderAsync(symbol, orderId: orderId);
Console.WriteLine(canceled.Success ? $"Canceled {orderId}." : $"Could not cancel {orderId}: {canceled.Error}");
