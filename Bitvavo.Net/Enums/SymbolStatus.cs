using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Symbol Status.
    /// </summary>
    [JsonConverter(typeof(EnumConverter<SymbolStatus>))]
    public enum SymbolStatus
    {
        /// <summary>
        /// ["<c>trading</c>"] Trading.
        /// </summary>
        [Map("trading")] 
        Trading,
        /// <summary>
        /// ["<c>halted</c>"] Halted.
        /// </summary>
        [Map("halted")]
        Halted,
        /// <summary>
        /// ["<c>auction</c>"] Auction.
        /// </summary>
        [Map("auction")]
        Auction,
        /// <summary>
        /// ["<c>auctionMatching</c>"] Auction Matching.
        /// </summary>
        [Map("auctionMatching")]
        AuctionMatching,
        /// <summary>
        /// ["<c>cancelOnly</c>"] Cancel Only.
        /// </summary>
        [Map("cancelOnly")]
        CancelOnly
    }
}
