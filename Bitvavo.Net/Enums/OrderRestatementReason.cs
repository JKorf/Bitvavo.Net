using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Reason Bitvavo changed an order
    /// </summary>
    [JsonConverter(typeof(EnumConverter<OrderRestatementReason>))]
    public enum OrderRestatementReason
    {
        /// <summary>
        /// ["<c>decrementOnSelfTradePrevention</c>"] Reduced to prevent a self-trade
        /// </summary>
        [Map("decrementOnSelfTradePrevention")]
        DecrementOnSelfTradePrevention,
        /// <summary>
        /// ["<c>cancelOnSelfTradePrevention</c>"] Canceled to prevent a self-trade
        /// </summary>
        [Map("cancelOnSelfTradePrevention")]
        CancelOnSelfTradePrevention,
        /// <summary>
        /// ["<c>cancelOnPlacementPriceProtection</c>"] Canceled by price protection on placement
        /// </summary>
        [Map("cancelOnPlacementPriceProtection")]
        CancelOnPlacementPriceProtection,
        /// <summary>
        /// ["<c>cancelOnExecutionPriceProtection</c>"] Canceled by price protection on execution
        /// </summary>
        [Map("cancelOnExecutionPriceProtection")]
        CancelOnExecutionPriceProtection,
        /// <summary>
        /// ["<c>cancelOnSpreadProtection</c>"] Canceled by spread protection
        /// </summary>
        [Map("cancelOnSpreadProtection")]
        CancelOnSpreadProtection,
        /// <summary>
        /// ["<c>cancelOnReferencePriceProtection</c>"] Canceled by reference-price protection
        /// </summary>
        [Map("cancelOnReferencePriceProtection")]
        CancelOnReferencePriceProtection,
        /// <summary>
        /// ["<c>cancelPostOnlyOnAuctionMatching</c>"] Post-only order canceled during auction matching
        /// </summary>
        [Map("cancelPostOnlyOnAuctionMatching")]
        CancelPostOnlyOnAuctionMatching,
        /// <summary>
        /// ["<c>cancelOnMaintenance</c>"] Canceled for maintenance
        /// </summary>
        [Map("cancelOnMaintenance")]
        CancelOnMaintenance,
        /// <summary>
        /// ["<c>cancelOnDisconnect</c>"] Canceled on disconnect
        /// </summary>
        [Map("cancelOnDisconnect")]
        CancelOnDisconnect,
        /// <summary>
        /// ["<c>cancelOnDelisting</c>"] Canceled on delisting
        /// </summary>
        [Map("cancelOnDelisting")]
        CancelOnDelisting,
        /// <summary>
        /// ["<c>cancelOnLockPlaced</c>"] Canceled when the account was locked
        /// </summary>
        [Map("cancelOnLockPlaced")]
        CancelOnLockPlaced,
        /// <summary>
        /// ["<c>cancelOnAdminRequest</c>"] Canceled by an administrator
        /// </summary>
        [Map("cancelOnAdminRequest")]
        CancelOnAdminRequest,
        /// <summary>
        /// ["<c>cancelPostOnly</c>"] Canceled because the post-only order would take liquidity
        /// </summary>
        [Map("cancelPostOnly")]
        CancelPostOnly,
    }
}
