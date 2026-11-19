using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Quantity notation in a MiCA report
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ReportQuantityNotation>))]
    public enum ReportQuantityNotation
    {
        /// <summary>
        /// ["<c>CRYP</c>"] Crypto asset quantity
        /// </summary>
        [Map("CRYP")]
        CryptoAsset,
    }
}
