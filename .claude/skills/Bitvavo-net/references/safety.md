# Operational considerations

## Credentials and environments

Configure `BitvavoCredentials` with an API key and secret. Use the appropriate permissions for account reads, trading, or withdrawals. Supply credentials to the WebSocket client for private queries and subscriptions. The client authenticates the connection.

`BitvavoEnvironment.Live` is the built-in environment. `CreateCustom` lets you specify service addresses. The configured addresses determine where requests execute.

## Orders and cancellations

Read the symbol metadata to check quantity limits and price increments before order placement. Limit order prices are in the quote asset. Use an order type and quantity field supported by the endpoint. Configure the transport's `OperatorId` before executing a request that requires one.

A limit order may execute immediately if it crosses the book. Use `postOnly` when the order should add liquidity exclusively. Keep the client order ID for correlation and subsequent lookups. Cancellation can race a fill. Reconcile the order and its fills after cancellation, and check the filter scope before using a bulk cancel.

## Withdrawals

Verify the asset, destination, network, and any required memo. The network-specific `WithdrawCryptoAsync` request accepts an `idempotencyKey` for retrying the same withdrawal details. Reconcile submitted withdrawals against the withdrawal history.

## Ambiguous outcomes and retries

For a lost order response or Bitvavo timeout code `109`, look up the order by client order ID or exchange order ID before deciding whether to submit again. Reconcile account state before repeating other mutations with an uncertain outcome.

REST rate-limit responses expose `ServerRateLimitError.RetryAfter` when the server provides timing information. Use that timestamp when retrying public reads, with a bounded retry policy and cancellation. See [error handling examples](usage.md#public-market-data-and-results).
