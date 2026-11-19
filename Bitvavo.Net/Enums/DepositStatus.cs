using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Deposit status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<DepositStatus>))]
    public enum DepositStatus
    {
        /// <summary>
        /// ["<c>completed</c>"] Transfer completed
        /// </summary>
        [Map("completed")]
        Completed,
        /// <summary>
        /// ["<c>canceled</c>"] Transfer canceled
        /// </summary>
        [Map("canceled")]
        Canceled,
    }
}
