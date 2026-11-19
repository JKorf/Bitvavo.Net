using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Price used to evaluate an order trigger
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TriggerReference>))]
    public enum TriggerReference
    {
        /// <summary>
        /// ["<c>lastTrade</c>"] Latest trade price
        /// </summary>
        [Map("lastTrade")]
        LastTrade,
        /// <summary>
        /// ["<c>bestBid</c>"] Best bid price
        /// </summary>
        [Map("bestBid")]
        BestBid,
        /// <summary>
        /// ["<c>bestAsk</c>"] Best ask price
        /// </summary>
        [Map("bestAsk")]
        BestAsk,
        /// <summary>
        /// ["<c>midPrice</c>"] Midpoint between the best bid and ask
        /// </summary>
        [Map("midPrice")]
        MidPrice,
    }
}
