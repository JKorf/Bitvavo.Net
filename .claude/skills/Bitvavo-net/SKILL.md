---
name: bitvavo-net
description: Use Bitvavo.Net in .NET applications for Bitvavo spot market data, account and transfer calls, order workflows, WebSocket subscriptions, and CryptoExchange.Net Shared API V2.
---

# Bitvavo.Net

Install with `dotnet add package JKorf.Bitvavo.Net`.

- Create `BitvavoRestClient` and `BitvavoSocketClient` from `Bitvavo.Net.Clients`. Both use `ExchangeApi`, with `ExchangeData`, `Account`, and `Trading` topics.
- Use native symbols such as `BTC-EUR` and asset codes such as `EUR`. Shared requests use `SharedSymbol(TradingMode.Spot, "BTC", "EUR")`.
- Set `ApiCredentials = new BitvavoCredentials(key, secret)` from `Bitvavo.Net` for private calls and subscriptions. Set the default `OperatorId` on the transport options for trading requests that require it.
- Use `ExchangeData` for public streams and `Trading.SubscribeToUserDataUpdatesAsync` for authenticated order and fill streams.
- Check `Success` before reading `Data` on `HttpResult<T>`, `QueryResult<T>`, and `WebSocketResult<UpdateSubscription>`. Close successful subscriptions with `subscription.Data.CloseAsync()`.
- Use `ExchangeApi.SharedApi` for exchange-independent operations. The DI aggregate `IBitvavoSharedApiClient` exposes `ExchangeRest` and `ExchangeSocket`.

## References

- Read [API surfaces](references/api-surfaces.md) to select topics, methods, and Shared API capabilities.
- Read [usage](references/usage.md) for focused C# snippets.
- Read [operational considerations](references/safety.md) for credentials, orders, withdrawals, and retries.

In a repository checkout, see the [development guide](../../../AGENTS.md) and [complete examples](../../../Examples/ai-friendly/).
