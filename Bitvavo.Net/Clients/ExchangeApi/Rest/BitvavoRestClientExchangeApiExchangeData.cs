using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using CryptoExchange.Net.Objects;
using Microsoft.Extensions.Logging;
using Bitvavo.Net.Interfaces.Clients.ExchangeApi;
using Bitvavo.Net.Objects.Models;

namespace Bitvavo.Net.Clients.ExchangeApi
{
    /// <inheritdoc />
    internal class BitvavoRestClientExchangeApiExchangeData : IBitvavoRestClientExchangeApiExchangeData
    {
        private readonly BitvavoRestClientExchangeApi _baseClient;
        private static readonly RequestDefinitionCache _definitions = new RequestDefinitionCache();

        internal BitvavoRestClientExchangeApiExchangeData(ILogger logger, BitvavoRestClientExchangeApi baseClient)
        {
            _baseClient = baseClient;
        }

        #region Get Trades

        /// <inheritdoc />
        public async Task<HttpResult<BitvavoTrade[]>> GetTradesAsync(
            string symbol,
            string? fromId = null, 
            string? toId = null,
            DateTime? startTime = null,
            DateTime? endTime = null, 
            int? limit = null, 
            CancellationToken ct = default)
        {
            var parameters = new Parameters(BitvavoExchange._parameterSerializationSettings);
            parameters.Add("limit", limit);
            parameters.Add("start", startTime);
            parameters.Add("end", endTime);
            parameters.Add("tradeIdFrom", fromId);
            parameters.Add("tradeIdTo", toId);

            var request = _definitions.GetOrCreate(HttpMethod.Get, _baseClient.BaseAddress, $"/v2/{symbol}/trades", BitvavoExchange.RateLimiter.Bitvavo, 1, false);
            return await _baseClient.SendAsync<BitvavoTrade[]>(request, null, ct).ConfigureAwait(false);
        }

        #endregion
    }
}
