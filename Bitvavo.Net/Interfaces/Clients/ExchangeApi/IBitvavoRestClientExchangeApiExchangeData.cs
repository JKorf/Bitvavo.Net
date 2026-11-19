using System;
using System.Threading;
using System.Threading.Tasks;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Objects;

namespace Bitvavo.Net.Interfaces.Clients.ExchangeApi
{
    /// <summary>
    /// Bitvavo Exchange exchange data endpoints. Exchange data includes market data (tickers, order books, etc) and system status.
    /// </summary>
    public interface IBitvavoRestClientExchangeApiExchangeData
    {
        /// <summary>
        /// Get trades in the last 24 hours
        /// <para>
        /// Docs:<br />
        /// <a href="https://docs.bitvavo.com/docs/rest-api/get-trades/" /><br />
        /// Endpoint:<br />
        /// GET /v2/{market}/trades
        /// </para>
        /// </summary>
        /// <param name="symbol">["<c>market</c>"] Filter by symbol, for example `ETH-USDC`</param>
        /// <param name="fromId">["<c>tradeIdFrom</c>"] Filter from id</param>
        /// <param name="toId">["<c>tradeIdTo</c>"] Filter to id</param>
        /// <param name="startTime">["<c>start</c>"] Filter by start time</param>
        /// <param name="endTime">["<c>end</c>"] Filter by end time</param>
        /// <param name="limit">["<c>limit</c>"] Max number of results, max 1000</param>
        /// <param name="ct">Cancellation token</param>
        Task<HttpResult<BitvavoTrade[]>> GetTradesAsync(
            string symbol,
            string? fromId = null,
            string? toId = null,
            DateTime? startTime = null,
            DateTime? endTime = null,
            int? limit = null,
            CancellationToken ct = default);
    }
}
