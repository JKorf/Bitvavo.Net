using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BitvavoRestClientExchangeApiTrading : IBitvavoRestClientExchangeApiTrading
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly BitvavoRestClientExchangeApi _baseClient;
        private readonly ILogger _logger;

        internal BitvavoRestClientExchangeApiTrading(ILogger logger, BitvavoRestClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
            _logger = logger;
        }
    }
}
