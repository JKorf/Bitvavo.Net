// 05-error-handling.cs
//
// Demonstrates reading errors from REST and WebSocket ticker requests.
//
// Install with dotnet add package JKorf.Bitvavo.Net

using Bitvavo.Net.Clients;
using CryptoExchange.Net.Objects;

var symbol = args.FirstOrDefault() ?? "BTC-EUR";
using var restClient = new BitvavoRestClient();
using var socketClient = new BitvavoSocketClient();

// REST requests return HttpResult<T>. Check Success before reading Data.
var restResult = await restClient.ExchangeApi.ExchangeData.GetTickerAsync(symbol);
if (restResult.Success)
    Console.WriteLine($"REST {symbol} last={restResult.Data.LastPrice}");
else
    PrintError("REST ticker", restResult.Error!);

// WebSocket requests return QueryResult<T> and expose the same Error properties.
var socketResult = await socketClient.ExchangeApi.ExchangeData.GetTickerAsync(symbol);
if (socketResult.Success)
    Console.WriteLine($"WebSocket {symbol} last={socketResult.Data.LastPrice}");
else
    PrintError("WebSocket ticker", socketResult.Error!);

static void PrintError(string operation, Error error)
{
    // ErrorType is a library classification. ErrorCode and Message come from Bitvavo when available.
    Console.Error.WriteLine($"{operation} failed ({error.ErrorType}, code {error.ErrorCode ?? "unavailable"}): {error.Message ?? error.ErrorDescription}");
    if (error is ServerRateLimitError { RetryAfter: { } retryAfter })
        Console.Error.WriteLine($"The rate limit resets at {retryAfter:u}.");
}
