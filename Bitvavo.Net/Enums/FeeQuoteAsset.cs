using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Quote asset for trading fees
    /// </summary>
    [JsonConverter(typeof(EnumConverter<FeeQuoteAsset>))]
    public enum FeeQuoteAsset
    {
        /// <summary>
        /// ["<c>EUR</c>"] Euro
        /// </summary>
        [Map("EUR")]
        Eur,
        /// <summary>
        /// ["<c>USDC</c>"] USD Coin
        /// </summary>
        [Map("USDC")]
        Usdc,
    }
}
