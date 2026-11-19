using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Order side in a MiCA report
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ReportOrderSide>))]
    public enum ReportOrderSide
    {
        /// <summary>
        /// ["<c>BUYI</c>"] Buy
        /// </summary>
        [Map("BUYI")]
        Buy,
        /// <summary>
        /// ["<c>SELL</c>"] Sell
        /// </summary>
        [Map("SELL")]
        Sell,
    }
}
