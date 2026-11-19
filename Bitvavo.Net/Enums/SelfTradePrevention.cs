using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Self-trade prevention mode
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SelfTradePrevention>))]
    public enum SelfTradePrevention
    {
        /// <summary>
        /// ["<c>cancelBoth</c>"] Cancel both orders
        /// </summary>
        [Map("cancelBoth")]
        CancelBoth,
        /// <summary>
        /// ["<c>cancelNewest</c>"] Cancel the newer order
        /// </summary>
        [Map("cancelNewest")]
        CancelNewest,
        /// <summary>
        /// ["<c>cancelOldest</c>"] Cancel the older order
        /// </summary>
        [Map("cancelOldest")]
        CancelOldest,
        /// <summary>
        /// ["<c>decrementAndCancel</c>"] Cancel the smaller order and reduce the larger order quantity
        /// </summary>
        [Map("decrementAndCancel")]
        DecrementAndCancel,
    }
}
