using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Trading status of a symbol
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolStatus>))]
    public enum SymbolStatus
    {
        /// <summary>
        /// ["<c>trading</c>"] Trading enabled
        /// </summary>
        [Map("trading")]
        Trading,
        /// <summary>
        /// ["<c>halted</c>"] Trading halted
        /// </summary>
        [Map("halted")]
        Halted,
        /// <summary>
        /// ["<c>auction</c>"] Auction in progress
        /// </summary>
        [Map("auction")]
        Auction,
        /// <summary>
        /// ["<c>auctionMatching</c>"] Auction orders are being matched
        /// </summary>
        [Map("auctionMatching")]
        AuctionMatching,
        /// <summary>
        /// ["<c>cancelOnly</c>"] Order cancellations only
        /// </summary>
        [Map("cancelOnly")]
        CancelOnly,
    }
}
