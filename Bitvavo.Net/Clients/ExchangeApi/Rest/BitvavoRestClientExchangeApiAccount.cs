using CryptoExchange.Net.Objects;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BitvavoRestClientExchangeApiAccount : IBitvavoRestClientExchangeApiAccount
    {
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();
        private readonly BitvavoRestClientExchangeApi _baseClient;

        internal BitvavoRestClientExchangeApiAccount(BitvavoRestClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
        }
    }
}
