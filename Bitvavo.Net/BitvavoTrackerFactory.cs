using CryptoExchange.Net.Authentication;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.Klines;
using CryptoExchange.Net.Trackers.Trades;
using CryptoExchange.Net.Trackers.UserData.Interfaces;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using System;
using Bitvavo.Net.Clients;
using Bitvavo.Net.Interfaces;
using Bitvavo.Net.Interfaces.Clients;

namespace Bitvavo.Net
{
    /// <inheritdoc />
    public class BitvavoTrackerFactory : IBitvavoTrackerFactory
    {
        private readonly IServiceProvider? _serviceProvider;

        /// <summary>
        /// ctor
        /// </summary>
        public BitvavoTrackerFactory()
        {
        }

        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="serviceProvider">Service provider for resolving logging and clients</param>
        public BitvavoTrackerFactory(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <inheritdoc />
        public bool CanCreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval)
        {
            var client = _serviceProvider?.GetRequiredService<IBitvavoSocketClient>() ?? new BitvavoSocketClient();
#warning TODO
            SubscribeKlineOptions klineOptions = new SubscribeKlineOptions(BitvavoExchange.Metadata.Id, true);
            return klineOptions.IsSupported(interval); 
        }

        /// <inheritdoc />
        public bool CanCreateTradeTracker(SharedSymbol symbol) => true;

        /// <inheritdoc />
        public IKlineTracker CreateKlineTracker(SharedSymbol symbol, SharedKlineInterval interval, int? limit = null, TimeSpan? period = null, ExchangeParameters? exchangeParameters = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IBitvavoRestClient>() ?? new BitvavoRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IBitvavoSocketClient>() ?? new BitvavoSocketClient();

#warning todo
            throw new NotImplementedException();
            //IKlineRestClient sharedRestClient;
            //IKlineSocketClient sharedSocketClient;
            //if (symbol.TradingMode == TradingMode.Spot)
            //{
            //    sharedRestClient = restClient.SpotApi.SharedClient;
            //    sharedSocketClient = socketClient.SpotApi.SharedClient;
            //}
            //else
            //{
            //    sharedRestClient = restClient.FuturesApi.SharedClient;
            //    sharedSocketClient = socketClient.FuturesApi.SharedClient;
            //}

            //return new KlineTracker(
            //    _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
            //    sharedRestClient,
            //    sharedSocketClient,
            //    symbol,
            //    interval,
            //    limit,
            //    period,
            //    exchangeParameters
            //    );
        }
        /// <inheritdoc />
        public ITradeTracker CreateTradeTracker(SharedSymbol symbol, int? limit = null, TimeSpan? period = null, ExchangeParameters? exchangeParameters = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IBitvavoRestClient>() ?? new BitvavoRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IBitvavoSocketClient>() ?? new BitvavoSocketClient();

#warning todo
            throw new NotImplementedException();

            //IRecentTradeRestClient? sharedRestClient;
            //ITradeSocketClient sharedSocketClient;
            //if (symbol.TradingMode == TradingMode.Spot)
            //{
            //    sharedRestClient = restClient.SpotApi.SharedClient;
            //    sharedSocketClient = socketClient.SpotApi.SharedClient;
            //}
            //else
            //{
            //    sharedRestClient = restClient.FuturesApi.SharedClient;
            //    sharedSocketClient = socketClient.FuturesApi.SharedClient;
            //}

            //return new TradeTracker(
            //    _serviceProvider?.GetRequiredService<ILoggerFactory>().CreateLogger(restClient.Exchange),
            //    sharedRestClient,
            //    null,
            //    sharedSocketClient,
            //    symbol,
            //    limit,
            //    period,
            //#warning check
            //    TradeQuantityType.BaseAsset,
            //    exchangeParameters
            //    );
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(SpotUserDataTrackerConfig? config = null, ExchangeParameters? exchangeParameters = null)
        {
            var restClient = _serviceProvider?.GetRequiredService<IBitvavoRestClient>() ?? new BitvavoRestClient();
            var socketClient = _serviceProvider?.GetRequiredService<IBitvavoSocketClient>() ?? new BitvavoSocketClient();
            return new BitvavoUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<BitvavoUserSpotDataTracker>>() ?? new NullLogger<BitvavoUserSpotDataTracker>(),
                restClient,
                socketClient,
                null,
                config,
                exchangeParameters
                );
        }

        /// <inheritdoc />
        public IUserSpotDataTracker CreateUserSpotDataTracker(string userIdentifier, BitvavoCredentials credentials, SpotUserDataTrackerConfig? config = null, BitvavoEnvironment? environment = null, ExchangeParameters? exchangeParameters = null)
        {
            var clientProvider = _serviceProvider?.GetRequiredService<IBitvavoUserClientProvider>() ?? new BitvavoUserClientProvider();
            var restClient = clientProvider.GetRestClient(userIdentifier, credentials, environment);
            var socketClient = clientProvider.GetSocketClient(userIdentifier, credentials, environment);
            return new BitvavoUserSpotDataTracker(
                _serviceProvider?.GetRequiredService<ILogger<BitvavoUserSpotDataTracker>>() ?? new NullLogger<BitvavoUserSpotDataTracker>(),
                restClient,
                socketClient,
                userIdentifier,
                config,
                exchangeParameters
                );
        }
    }
}
