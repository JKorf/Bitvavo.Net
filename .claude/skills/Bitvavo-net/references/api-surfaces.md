# API surfaces

## Client roots and symbols

Both `BitvavoRestClient` and `BitvavoSocketClient` use `ExchangeApi`. This library exposes spot markets. Use `ExchangeData` for public market data, `Account` for balances and transfers, and `Trading` for orders and user trades.

Native symbols use `BASE-QUOTE`, for example `BTC-EUR`. Account asset filters use codes such as `BTC` and `EUR`. Use `GetSymbolsAsync` for supported markets and `GetSymbolAsync` for tick size, quantity precision, and order limits. Use `KlineInterval` for candles and `OrderSide`, `OrderType`, and `TimeInForce` for order settings.

## Market data

Use the `ExchangeData` topic on either transport unless indicated.

| Intent | Method |
| --- | --- |
| Server time | `GetServerTimeAsync()` |
| Symbols | `GetSymbolsAsync()`, `GetSymbolAsync(symbol)` |
| Assets and networks | `GetAssetsAsync()`, `GetAssetAsync(asset)` |
| Last trade price | `GetTickerPriceAsync(symbol)`, `GetTickerPricesAsync()` |
| Best bid and ask | `GetBookTickerAsync(symbol)`, `GetBookTickersAsync()` |
| 24-hour statistics | `GetTickerAsync(symbol)`, `GetTickersAsync()` |
| Candles | `GetKlinesAsync(symbol, KlineInterval.OneMinute, ...)` |
| Order book snapshot | `GetOrderBookAsync(symbol, depth: ...)` |
| Public trades | `GetTradesAsync(symbol, ...)` |
| Market data reports | REST `GetOrderBookReportAsync(symbol, ...)`, `GetTradesReportAsync(symbol, ...)` |

## Account and transfers

Use the authenticated `Account` topic on either transport unless indicated.

| Intent | Method |
| --- | --- |
| All balances or one asset | `GetBalancesAsync(asset: ...)` |
| Staking balances | REST `GetStakingBalancesAsync(asset: ...)` |
| Account fee tiers | `GetAccountFeesAsync()` |
| Fees by symbol or quote asset | `GetSymbolFeesAsync(symbol: ...)` or `GetSymbolFeesAsync(quoteAsset: ...)` |
| Paginated transaction history | `GetTransactionHistoryAsync(..., page: ..., pageSize: ...)` |
| Deposit details | `GetDepositDataAsync(asset)` |
| Deposit and withdrawal history | `GetDepositHistoryAsync(...)`, `GetWithdrawalHistoryAsync(...)` |
| Withdraw to a wallet or bank account | `WithdrawAsync(asset, address, quantity, ...)` |
| Withdraw crypto on a specific network | `WithdrawCryptoAsync(asset, network, address, quantity, ...)` |

## Trading

Use the authenticated `Trading` topic on either transport for these queries.

| Intent | Method |
| --- | --- |
| Place a spot order | `PlaceOrderAsync(symbol, side, type, ...)` |
| Get, amend, or cancel one order | `GetOrderAsync`, `UpdateOrderAsync`, or `CancelOrderAsync` with `symbol` and either `orderId` or `clientOrderId` |
| Open orders | `GetOpenOrdersAsync(symbol: ...)` or `GetOpenOrdersAsync(baseAsset: ...)` |
| Order and fill history | `GetOrderHistoryAsync(symbol, ...)`, `GetUserTradesAsync(symbol, ...)` |
| Cancel all or by symbol | `CancelOrdersAsync()`, `CancelOrdersAsync(symbol)` |
| Atomically cancel by symbol or side | `CancelOrdersAtomicAsync(...)` |
| Cancel-on-disconnect timer | `CancelOrdersAfterAsync(cancelGroupId, expiryAfterSeconds)` |

`quantity` is in the base asset and `quoteQuantity` is in the quote asset. Select the field supported by the order type. Use `GetSymbolAsync` for precision and limits. Requests that take an optional `operatorId` use the configured transport default when the argument is omitted.

## WebSocket subscriptions

| Intent | Topic and method |
| --- | --- |
| Best bid, ask, and last trade price changes | `ExchangeData.SubscribeToTickerUpdatesAsync` |
| 24-hour ticker, one callback per symbol update | `ExchangeData.SubscribeToTicker24hUpdatesAsync` |
| 24-hour tickers grouped by received message | `ExchangeData.SubscribeToTicker24hBatchUpdatesAsync` |
| Candles | `ExchangeData.SubscribeToKlineUpdatesAsync` |
| Public trades | `ExchangeData.SubscribeToTradeUpdatesAsync` |
| Incremental book changes | `ExchangeData.SubscribeToOrderBookUpdatesAsync` |
| Authenticated orders and fills | `Trading.SubscribeToUserDataUpdatesAsync` |

Batch ticker updates can contain only some subscribed symbols. The native ticker stream can report on one side of the book at a time. For a synchronized full book, use `IBitvavoOrderBookFactory.Create(symbol)`; the native book stream reports deltas with a nonce. The private order and fill stream accepts a symbol, a set of symbols, or both callbacks with no symbol filter. Pass `null` for an individual callback you do not need.

## Shared API V2

Use `restClient.ExchangeApi.SharedApi` or `socketClient.ExchangeApi.SharedApi`. The DI aggregate `IBitvavoSharedApiClient` exposes the same surfaces as `ExchangeRest` and `ExchangeSocket`.

| Intent | Capability |
| --- | --- |
| Spot symbols and cached catalog | `IGetSpotSymbolsRest`, `SpotSymbolCatalog` |
| Tickers, top of book, and book snapshots | `IGetTickerRest`, `IGetAllTickersRest`, `IGetBookTickerRest`, `IGetOrderBookRest` |
| Candles and public trades | `IGetKlinesRest`, `IGetRecentTradesRest`, `IGetTradeHistoryRest` |
| Assets, balances, fees, and ledger | `IGetAssetRest`, `IGetAllAssetsRest`, `IGetBalancesRest`, `IGetFeesRest`, `IGetLedgerRest` |
| Deposit addresses, transfers, and history | `IGetDepositAddressesRest`, `IGetDepositHistoryRest`, `IWithdrawRest`, `IGetWithdrawalHistoryRest` |
| Spot placement, edits, and cancellations | `IPlaceSpotOrderRest`, `IEditSpotOrderRest`, `IEditSpotOrderByClientOrderIdRest`, `ICancelSpotOrderRest`, `ICancelSpotOrderByClientOrderIdRest`, `ICancelAllSpotOrdersRest`, `ICancelAllSpotSymbolOrdersRest` |
| Spot order lookups and fills | `IGetSpotOrderRest`, `IGetSpotOrderByClientOrderIdRest`, `IGetOpenSpotOrdersRest`, `IGetClosedSpotOrdersRest`, `IGetSpotUserTradeHistoryRest`, `IGetSpotOrderTradesRest` |
| Spot trigger orders | `IPlaceSpotTriggerOrderRest`, `IGetSpotTriggerOrderRest`, `ICancelSpotTriggerOrderRest` |
| WebSocket spot order queries | `IPlaceSpotOrderSocket`, `IEditSpotOrderSocket`, `IEditSpotOrderByClientOrderIdSocket`, `ICancelSpotOrderSocket`, `ICancelSpotOrderByClientOrderIdSocket`, `ICancelAllSpotOrdersSocket`, `ICancelAllSpotSymbolOrdersSocket` |
| WebSocket market subscriptions | `ISubscribeTickerSocket`, `ISubscribeAllTickersSocket`, `ISubscribeBookTickerSocket`, `ISubscribeKlinesSocket`, `ISubscribeIncrementalOrderBookSocket`, `ISubscribeTradesSocket` |
| WebSocket private subscriptions | `ISubscribeSpotOrdersSocket`, `ISubscribeUserTradesSocket` |

Use `Discover()` or `GetCapability<T>(TradingMode.Spot, transport)` to inspect support and check for a match before using its `Capability` and `Options`. Use the native WebSocket topics for queries such as balances or market snapshots.

Load symbols with `GetSpotSymbolsAsync(new GetSymbolsRequest())` before relying on the cached symbol catalog. Use shared quantity types when placing orders and inspect the capability options for supported units. Where supported, set the per-request operator override with `ExchangeParameters.AddValue("Bitvavo", "operatorId", operatorId)`.

The all-tickers subscription uses the symbols available when it opens. Shared book tickers wait until both bid and ask are available. Shared incremental order book updates contain only deltas and have no depth filter.

## Source navigation in a checkout

- [Public client interfaces](../../../../Bitvavo.Net/Interfaces/Clients/) contain exact parameter types and overloads.
- [Local order books](../../../../Bitvavo.Net/SymbolOrderBooks/) maintain synchronized books.
- [Tracker factory](../../../../Bitvavo.Net/BitvavoTrackerFactory.cs) and [user client provider](../../../../Bitvavo.Net/Clients/BitvavoUserClientProvider.cs) support higher-level user workflows.
