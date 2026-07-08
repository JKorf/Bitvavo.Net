using System;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using Bitvavo.Net.Clients;
using Bitvavo.Net.Interfaces.Clients;
using Bitvavo.Net.Objects.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Bitvavo.Net.Objects.Models.Spot;

namespace Bitvavo.Net.SymbolOrderBooks
{
    /// <summary>
    /// Bitvavo order book implementation
    /// </summary>
    public class BitvavoSpotSymbolOrderBook: CryptoExchange.Net.OrderBook.SymbolOrderBook
    {
        private readonly IBitvavoRestClient _restClient;
        private readonly IBitvavoSocketClient _socketClient;
        private readonly TimeSpan _initialDataTimeout;
        private readonly bool _clientOwner;

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsFunc">The options for the order book</param>
        public BitvavoSpotSymbolOrderBook(string symbol, Action<BitvavoOrderBookOptions>? optionsFunc = null)
            : this(symbol, optionsFunc, null, null, null)
        {
            _clientOwner = true;
        }

        /// <summary>
        /// Create a new order book instance
        /// </summary>
        /// <param name="symbol">The symbol the order book is for</param>
        /// <param name="optionsFunc">The options for the order book</param>
        /// <param name="logger">Logger</param>
        /// <param name="restClient">Rest client instance</param>
        /// <param name="socketClient">Socket client instance</param>
        [ActivatorUtilitiesConstructor]
        public BitvavoSpotSymbolOrderBook(
            string symbol,
            Action<BitvavoOrderBookOptions>? optionsFunc,
            ILoggerFactory? logger,
            IBitvavoRestClient? restClient,
            IBitvavoSocketClient? socketClient) : base(logger, "Bitvavo", "Spot", symbol)
        {
            var options = BitvavoOrderBookOptions.Default.Copy();
            if (optionsFunc != null)
                optionsFunc(options);
            Initialize(options);

            _strictLevels = false;
            _sequencesAreConsecutive = true;

            Levels = options.Limit;
            _initialDataTimeout = options.InitialDataTimeout ?? TimeSpan.FromSeconds(30);
            _clientOwner = socketClient == null;
            _socketClient = socketClient ?? new BitvavoSocketClient();
            _restClient = restClient ?? new BitvavoRestClient();
        }

        /// <inheritdoc />
        protected override async Task<CallResult<UpdateSubscription>> DoStartAsync(CancellationToken ct)
        {
            var subResult = await _socketClient.SpotApi.ExchangeData.SubscribeToOrderBookUpdatesAsync(new[] { Symbol }, HandleUpdate).ConfigureAwait(false);
            if (!subResult.Success)
                return CallResult.Fail<UpdateSubscription>(subResult.Error);

            if (ct.IsCancellationRequested)
            {
                await subResult.Data.CloseAsync().ConfigureAwait(false);
                return CallResult.Fail<UpdateSubscription>(new CancellationRequestedError());
            }

            Status = OrderBookStatus.Syncing;

            // Wait up to 1s until the first update has been received
            await WaitUntilFirstUpdateBufferedAsync(TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(1000), ct).ConfigureAwait(false);

            var bookResult = await _restClient.SpotApi.ExchangeData.GetOrderBookAsync(Symbol).ConfigureAwait(false);
            if (!bookResult.Success)
            {
                _logger.Log(LogLevel.Debug, $"{Api} order book {Symbol} failed to retrieve initial order book: " + bookResult.Error);
                await _socketClient.UnsubscribeAsync(subResult.Data).ConfigureAwait(false);
                return CallResult.Fail<UpdateSubscription>(bookResult.Error!);
            }

            SetSnapshot(bookResult.Data.Nonce, bookResult.Data.Bids, bookResult.Data.Asks);
            
            return CallResult.Ok(subResult.Data);
        }

        /// <inheritdoc />
        protected override async Task<CallResult> DoResyncAsync(CancellationToken ct)
        {
            // Wait up to 1s until the first update has been received
            await WaitUntilFirstUpdateBufferedAsync(TimeSpan.FromMilliseconds(1000), TimeSpan.FromMilliseconds(1000), ct).ConfigureAwait(false);

            var bookResult = await _restClient.SpotApi.ExchangeData.GetOrderBookAsync(Symbol).ConfigureAwait(false);
            if (!bookResult.Success)
                return CallResult.Fail(bookResult.Error!);

            SetSnapshot(bookResult.Data.Nonce, bookResult.Data.Bids, bookResult.Data.Asks);
            return CallResult.Ok();
        }

        private void HandleUpdate(DataEvent<BitvavoOrderBookUpdate> data)
        {
            UpdateOrderBook(data.Data.Nonce, data.Data.Bids, data.Data.Asks, data.DataTime, data.DataTimeLocal);
        }

        /// <summary>
        /// Dispose
        /// </summary>
        protected override void Dispose(bool disposing)
        {
            if (_clientOwner)
            {
                _socketClient?.Dispose();
                _restClient?.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
