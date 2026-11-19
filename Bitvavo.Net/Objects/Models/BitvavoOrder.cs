using Bitvavo.Net.Enums;
using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Order details. Fields omitted from a compact response are left unset
    /// </summary>
    public record BitvavoOrder
    {
        /// <summary>
        /// ["<c>orderId</c>"] Bitvavo order ID
        /// </summary>
        [JsonPropertyName("orderId")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>clientOrderId</c>"] Client order ID
        /// </summary>
        [JsonPropertyName("clientOrderId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>market</c>"] Trading symbol
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>created</c>"] Order creation time
        /// </summary>
        [JsonPropertyName("created")]
        public DateTime? CreateTime { get; set; }
        /// <summary>
        /// ["<c>updated</c>"] Last update time
        /// </summary>
        [JsonPropertyName("updated")]
        public DateTime? UpdateTime { get; set; }
        /// <summary>
        /// ["<c>createdNs</c>"] Order creation time as Unix nanoseconds
        /// </summary>
        [JsonPropertyName("createdNs")]
        public long? CreateTimeNanoseconds { get; set; }
        /// <summary>
        /// ["<c>updatedNs</c>"] Last update time as Unix nanoseconds
        /// </summary>
        [JsonPropertyName("updatedNs")]
        public long? UpdateTimeNanoseconds { get; set; }
        /// <summary>
        /// ["<c>status</c>"] Order status
        /// </summary>
        [JsonPropertyName("status")]
        public OrderStatus? Status { get; set; }
        /// <summary>
        /// ["<c>side</c>"] Order side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide? Side { get; set; }
        /// <summary>
        /// ["<c>orderType</c>"] Order type
        /// </summary>
        [JsonPropertyName("orderType")]
        public OrderType? Type { get; set; }
        /// <summary>
        /// ["<c>amount</c>"] Order quantity in the base asset
        /// </summary>
        [JsonPropertyName("amount")]
        public decimal? Quantity { get; set; }
        /// <summary>
        /// ["<c>amountRemaining</c>"] Remaining order quantity in the base asset
        /// </summary>
        [JsonPropertyName("amountRemaining")]
        public decimal? RemainingQuantity { get; set; }
        /// <summary>
        /// ["<c>price</c>"] Limit price
        /// </summary>
        [JsonPropertyName("price")]
        public decimal? Price { get; set; }
        /// <summary>
        /// ["<c>amountQuote</c>"] Order quantity in the quote asset
        /// </summary>
        [JsonPropertyName("amountQuote")]
        public decimal? QuoteQuantity { get; set; }
        /// <summary>
        /// ["<c>amountQuoteRemaining</c>"] Remaining order quantity in the quote asset
        /// </summary>
        [JsonPropertyName("amountQuoteRemaining")]
        public decimal? RemainingQuoteQuantity { get; set; }
        /// <summary>
        /// ["<c>onHold</c>"] Quantity held while the order settles
        /// </summary>
        [JsonPropertyName("onHold")]
        public decimal? OnHoldQuantity { get; set; }
        /// <summary>
        /// ["<c>onHoldCurrency</c>"] Asset held while the order settles
        /// </summary>
        [JsonPropertyName("onHoldCurrency")]
        public string? OnHoldAsset { get; set; }
        /// <summary>
        /// ["<c>triggerPrice</c>"] Calculated trigger price
        /// </summary>
        [JsonPropertyName("triggerPrice")]
        public decimal? TriggerPrice { get; set; }
        /// <summary>
        /// ["<c>triggerAmount</c>"] Trigger price specified in the request
        /// </summary>
        [JsonPropertyName("triggerAmount")]
        public decimal? RequestedTriggerPrice { get; set; }
        /// <summary>
        /// ["<c>triggerType</c>"] Type of trigger
        /// </summary>
        [JsonPropertyName("triggerType")]
        public TriggerType? TriggerType { get; set; }
        /// <summary>
        /// ["<c>triggerReference</c>"] Price used to evaluate the trigger
        /// </summary>
        [JsonPropertyName("triggerReference")]
        public TriggerReference? TriggerReference { get; set; }
        /// <summary>
        /// ["<c>filledAmount</c>"] Filled quantity in the base asset
        /// </summary>
        [JsonPropertyName("filledAmount")]
        public decimal? FilledQuantity { get; set; }
        /// <summary>
        /// ["<c>filledAmountQuote</c>"] Filled quantity in the quote asset
        /// </summary>
        [JsonPropertyName("filledAmountQuote")]
        public decimal? FilledQuoteQuantity { get; set; }
        /// <summary>
        /// ["<c>feePaid</c>"] Fee paid or negative rebate
        /// </summary>
        [JsonPropertyName("feePaid")]
        public decimal? FeeQuantity { get; set; }
        /// <summary>
        /// ["<c>feeCurrency</c>"] Asset in which the fee is paid
        /// </summary>
        [JsonPropertyName("feeCurrency")]
        public string? FeeAsset { get; set; }
        /// <summary>
        /// ["<c>fills</c>"] Order fills when returned
        /// </summary>
        [JsonPropertyName("fills")]
        public BitvavoOrderFill[]? Fills { get; set; }
        /// <summary>
        /// ["<c>selfTradePrevention</c>"] Self-trade prevention mode
        /// </summary>
        [JsonPropertyName("selfTradePrevention")]
        public SelfTradePrevention? SelfTradePrevention { get; set; }
        /// <summary>
        /// ["<c>visible</c>"] Whether the order is visible in the order book
        /// </summary>
        [JsonPropertyName("visible")]
        public bool? Visible { get; set; }
        /// <summary>
        /// ["<c>timeInForce</c>"] Time in force
        /// </summary>
        [JsonPropertyName("timeInForce")]
        public TimeInForce? TimeInForce { get; set; }
        /// <summary>
        /// ["<c>postOnly</c>"] Whether the order can only add liquidity
        /// </summary>
        [JsonPropertyName("postOnly")]
        public bool? PostOnly { get; set; }
        /// <summary>
        /// ["<c>disableMarketProtection</c>"] Legacy market protection flag
        /// </summary>
        [JsonPropertyName("disableMarketProtection")]
        public bool? DisableMarketProtection { get; set; }
        /// <summary>
        /// ["<c>operatorId</c>"] Identifier of the trader or bot that submitted the request
        /// </summary>
        [JsonPropertyName("operatorId")]
        public long? OperatorId { get; set; }
        /// <summary>
        /// ["<c>restatementReason</c>"] Reason Bitvavo changed the order
        /// </summary>
        [JsonPropertyName("restatementReason")]
        public OrderRestatementReason? RestatementReason { get; set; }
        /// <summary>
        /// ["<c>codGroupId</c>"] Cancel-on-disconnect group ID
        /// </summary>
        [JsonPropertyName("codGroupId")]
        public int? CancelGroupId { get; set; }
    }
}
