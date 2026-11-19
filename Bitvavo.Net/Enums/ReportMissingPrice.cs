using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Reason a MiCA trade report price is unavailable
    /// </summary>
    [JsonConverter(typeof(EnumConverter<ReportMissingPrice>))]
    public enum ReportMissingPrice
    {
        /// <summary>
        /// ["<c>PNDG</c>"] Price pending
        /// </summary>
        [Map("PNDG")]
        Pending,
        /// <summary>
        /// ["<c>NOAP</c>"] Price not applicable
        /// </summary>
        [Map("NOAP")]
        NotApplicable,
    }
}
