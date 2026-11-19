using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Trackers.UserData;
using CryptoExchange.Net.Trackers.UserData.Objects;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Bitvavo.Net.Interfaces.Clients;

namespace Bitvavo.Net
{
    /// <inheritdoc />
    public class BitvavoUserSpotDataTracker : UserSpotDataTracker
    {
        /// <summary>
        /// ctor
        /// </summary>
        public BitvavoUserSpotDataTracker(
            ILogger<BitvavoUserSpotDataTracker> logger,
            IBitvavoRestClient restClient,
            IBitvavoSocketClient socketClient,
            string? userIdentifier,
            SpotUserDataTrackerConfig? config = null,
            ExchangeParameters? exchangeParameters = null) : base(
                logger,
#warning TODO
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                null,
                userIdentifier,
                config ?? new SpotUserDataTrackerConfig(),
                exchangeParameters)
        {

        }
    }
}
