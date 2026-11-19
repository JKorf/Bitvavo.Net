using Bitvavo.Net.Clients;
using Bitvavo.Net.Interfaces.Clients;
using Bitvavo.Net.Objects.Models;
using Bitvavo.Net.Objects.Options;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.OrderBook;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Bitvavo.Net.SymbolOrderBooks
{
    /// <summary>
    /// Implementation for a synchronized order book. After calling Start the order book will sync itself and keep up to date with new data. It will automatically try to reconnect and resync in case of a lost/interrupted connection.
    /// Make sure to check the State property to see if the order book is synced.
    /// </summary>
    public class BitvavoSymbolOrderBook : SymbolOrderBook
    {
        private readonly bool _clientOwner;
        private readonly IBitvavoSocketClient _socketClient;
        private readonly TimeSpan _initialDataTimeout;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        public BitvavoSymbolOrderBook(string symbol, Action<BitvavoOrderBookOptions>? optionsDelegate = null)
            : this(symbol, optionsDelegate, null, null)
        {
            _clientOwner = true;
        }

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsDelegate">Option configuration delegate</param>
        /// <param name="logger">Logger</param>
        /// <param name="socketClient">Socket client instance</param>
        public BitvavoSymbolOrderBook(
            string symbol,
            Action<BitvavoOrderBookOptions>? optionsDelegate,
            ILoggerFactory? logger,
            IBitvavoSocketClient? socketClient) : base(logger, "Bitvavo", "Exchange", symbol)
        {
            var options = BitvavoOrderBookOptions.Default.Copy();
            if (optionsDelegate != null)
                optionsDelegate(options);
            Initialize(options);

            _strictLevels = false;
            _sequencesAreConsecutive = true;

            Levels = options?.Limit;
            _initialDataTimeout = options?.InitialDataTimeout ?? TimeSpan.FromSeconds(30);
            _clientOwner = socketClient == null;
            _socketClient = socketClient ?? new BitvavoSocketClient();
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStartAsync(CancellationToken ct)
        {
            var subResult = await _socketClient.ExchangeApi.ExchangeData.SubscribeToOrderBookUpdatesAsync(Symbol, HandleUpdate).ConfigureAwait(false);
            if (!subResult.Success)
                return CallResult<UpdateSubscription>.Fail(subResult.Error!);

            if (ct.IsCancellationRequested)
            {
                await subResult.Data.CloseAsync().ConfigureAwait(false);
                return CallResult<UpdateSubscription>.Fail(new CancellationRequestedError());
            }

            Status = OrderBookStatus.Syncing;

            // Wait up to 250ms until the first update has been received
            await WaitUntilFirstUpdateBufferedAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(250), ct).ConfigureAwait(false);

            var bookResult = await _socketClient.ExchangeApi.ExchangeData.GetOrderBookAsync(Symbol, 1000).ConfigureAwait(false);
            if (!bookResult.Success)
            {
                _logger.Log(LogLevel.Debug, $"{Api} order book {Symbol} failed to retrieve initial order book");
                await _socketClient.UnsubscribeAsync(subResult.Data).ConfigureAwait(false);
                return CallResult<UpdateSubscription>.Fail(bookResult.Error!);
            }

            SetSnapshot(bookResult.Data.Nonce, bookResult.Data.Bids, bookResult.Data.Asks);
            return CallResult<UpdateSubscription>.Ok(subResult.Data);
        }

        private void HandleUpdate(DataEvent<BitvavoOrderBook> data)
        {
            UpdateOrderBook(data.Data.Nonce, data.Data.Bids, data.Data.Asks, data.DataTime, data.DataTimeLocal);
        }

        /// <inheritdoc />
        protected override void DoReset()
        {
        }

        /// <inheritdoc />
        protected override async Task<CallResult> DoResyncAsync(CancellationToken ct)
        {
            // Wait up to 250ms until the first update has been received
            await WaitUntilFirstUpdateBufferedAsync(TimeSpan.FromMilliseconds(100), TimeSpan.FromMilliseconds(250), ct).ConfigureAwait(false);

            var bookResult = await _socketClient.ExchangeApi.ExchangeData.GetOrderBookAsync(Symbol, Levels ?? 1000).ConfigureAwait(false);
            if (!bookResult.Success)
                return CallResult.Fail(bookResult.Error!);

            SetSnapshot(bookResult.Data.Nonce, bookResult.Data.Bids, bookResult.Data.Asks);
            return CallResult.Ok();
        }

        /// <inheritdoc />
        protected override void Dispose(bool disposing)
        {
            if (_clientOwner)
                _socketClient?.Dispose();

            base.Dispose(disposing);
        }
    }
}
