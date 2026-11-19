using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Type of a WebSocket order update
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderExecutionType>))]
    public enum OrderExecutionType
    {
        /// <summary>
        /// ["<c>new</c>"] Order created
        /// </summary>
        [Map("new")]
        New,
        /// <summary>
        /// ["<c>trade</c>"] Order filled in whole or in part
        /// </summary>
        [Map("trade")]
        Trade,
        /// <summary>
        /// ["<c>canceled</c>"] Order canceled
        /// </summary>
        [Map("canceled")]
        Canceled,
        /// <summary>
        /// ["<c>expired</c>"] Order expired
        /// </summary>
        [Map("expired")]
        Expired,
        /// <summary>
        /// ["<c>updated</c>"] Order updated
        /// </summary>
        [Map("updated")]
        Updated,
        /// <summary>
        /// ["<c>triggered</c>"] Order triggered
        /// </summary>
        [Map("triggered")]
        Triggered,
        /// <summary>
        /// ["<c>restated</c>"] Order restated
        /// </summary>
        [Map("restated")]
        Restated,
    }
}
