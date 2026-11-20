# Bitvavo.Net console examples

Each C# file is a standalone console program. Create a console project with the .NET SDK, add the Bitvavo.Net package, and copy one example into the project as `Program.cs`.

```sh
dotnet new console -o BitvavoSample
cd BitvavoSample
dotnet add package JKorf.Bitvavo.Net
```

| Example | What it demonstrates |
| --- | --- |
| [Market data and account](01-unified-quickstart.cs) | Query a spot symbol, its 24-hour ticker and candles, and optionally its quote-asset balance. |
| [Spot trading](02-spot-trading.cs) | Submit, inspect, and cancel a post-only limit order using the REST trading API. |
| [WebSocket subscriptions](03-websocket.cs) | Receive 24-hour ticker updates in batches for BTC-EUR and ETH-EUR. |
| [Shared API](04-multi-exchange.cs) | Register Bitvavo.Net with dependency injection and load a spot ticker through the exchange-independent `IGetTickerRest` capability. |
| [Error handling](05-error-handling.cs) | Inspect error codes, classifications, and rate-limit reset times from REST and WebSocket results. |

Run public examples with `dotnet run`. To include balances in the first example, set `BITVAVO_API_KEY` and `BITVAVO_API_SECRET`.

The spot trading example submits a live BTC-EUR order. Set both credentials and `BITVAVO_OPERATOR_ID`, then pass the base-asset quantity and quote-asset limit price as `dotnet run -- <quantity-in-BTC> <limit-price-in-EUR>`.

The repository's example test compiles each program independently without running it.
