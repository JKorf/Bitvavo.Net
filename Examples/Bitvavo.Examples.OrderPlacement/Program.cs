using Bitvavo.Net;
using Bitvavo.Net.Clients;
using Bitvavo.Net.Enums;

const string spotSymbol = "ETH-EUR";

// Replace with valid credentials or order placement will always fail
var apiKey = "KEY";
var apiSecret = "SECRET";

Console.WriteLine("Bitvavo.Net order placement example");
Console.WriteLine();
Console.WriteLine("This example can place real orders when valid credentials are configured.");
Console.WriteLine();

var client = new BitvavoRestClient(options =>
{
    options.ApiCredentials = new BitvavoCredentials(apiKey, apiSecret);
});

await PlaceSpotLimitOrderAsync(client);

static async Task PlaceSpotLimitOrderAsync(BitvavoRestClient client)
{
    Console.WriteLine($"Placing spot limit buy order for {spotSymbol}...");

    var ticker = await client.ExchangeApi.ExchangeData.GetTickerAsync(spotSymbol);
    if (!ticker.Success || ticker.Data.LastPrice == null)
    {
        Console.WriteLine($"Failed to get spot ticker: {ticker.Error}");
        return;
    }

    var safePrice = Math.Round(ticker.Data.LastPrice.Value * 0.95m, 2);
    var order = await client.ExchangeApi.Trading.PlaceOrderAsync(
        symbol: spotSymbol,
        side: OrderSide.Buy,
        type: OrderType.Limit,
        quantity: 0.001m,
        price: safePrice);

    if (!order.Success)
    {
        Console.WriteLine($"Failed to place spot order: {order.Error}");
        return;
    }

    Console.WriteLine($"Placed spot order {order.Data.Id}, status: {order.Data.Status}");

    var orderStatus = await client.ExchangeApi.Trading.GetOrderAsync(spotSymbol, orderId: order.Data.Id);
    if (orderStatus.Success)
        Console.WriteLine($"Spot order status: {orderStatus.Data.Status}, filled: {orderStatus.Data.FilledQuantity}");
    else
        Console.WriteLine($"Failed to query spot order: {orderStatus.Error}");

    var cancel = await client.ExchangeApi.Trading.CancelOrderAsync(spotSymbol, orderId: order.Data.Id);
    Console.WriteLine(cancel.Success
        ? $"Cancelled spot order {order.Data.Id}"
        : $"Failed to cancel spot order: {cancel.Error}");
}
