using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Order status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderStatus>))]
    public enum OrderStatus
    {
        /// <summary>
        /// ["<c>new</c>"] New order
        /// </summary>
        [Map("new")]
        New,
        /// <summary>
        /// ["<c>awaitingTrigger</c>"] Waiting for the trigger
        /// </summary>
        [Map("awaitingTrigger")]
        AwaitingTrigger,
        /// <summary>
        /// ["<c>canceled</c>"] Canceled order
        /// </summary>
        [Map("canceled")]
        Canceled,
        /// <summary>
        /// ["<c>expired</c>"] Expired order
        /// </summary>
        [Map("expired")]
        Expired,
        /// <summary>
        /// ["<c>filled</c>"] Fully filled order
        /// </summary>
        [Map("filled")]
        Filled,
        /// <summary>
        /// ["<c>partiallyFilled</c>"] Partially filled order
        /// </summary>
        [Map("partiallyFilled")]
        PartiallyFilled,
    }
}
