using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Price notation in a MiCA report
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ReportPriceNotation>))]
    public enum ReportPriceNotation
    {
        /// <summary>
        /// ["<c>MONE</c>"] Monetary value
        /// </summary>
        [Map("MONE")]
        MonetaryValue,
    }
}
