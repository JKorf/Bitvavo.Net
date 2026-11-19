using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Deposit or withdrawal status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<AssetTransferStatus>))]
    public enum AssetTransferStatus
    {
        /// <summary>
        /// ["<c>OK</c>"] Transfers enabled
        /// </summary>
        [Map("OK")]
        Ok,
        /// <summary>
        /// ["<c>MAINTENANCE</c>"] Transfers suspended for maintenance
        /// </summary>
        [Map("MAINTENANCE")]
        Maintenance,
        /// <summary>
        /// ["<c>DELISTED</c>"] Asset delisted
        /// </summary>
        [Map("DELISTED")]
        Delisted,
    }
}
