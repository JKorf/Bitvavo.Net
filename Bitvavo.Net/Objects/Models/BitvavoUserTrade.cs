using Bitvavo.Net.Enums;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Trade in the account trade history
    /// </summary>
    public record BitvavoUserTrade : BitvavoOrderFill
    {
        /// <summary>
        /// ["<c>orderId</c>"] Bitvavo order ID
        /// </summary>
        [JsonPropertyName("orderId")]
        public string OrderId { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>clientOrderId</c>"] Client order ID
        /// </summary>
        [JsonPropertyName("clientOrderId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>operatorId</c>"] Identifier of the trader or bot
        /// </summary>
        [JsonPropertyName("operatorId")]
        public long? OperatorId { get; set; }
        /// <summary>
        /// ["<c>market</c>"] Trading symbol
        /// </summary>
        [JsonPropertyName("market")]
        public string Symbol { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>side</c>"] Trade side
        /// </summary>
        [JsonPropertyName("side")]
        public OrderSide Side { get; set; }
    }
}
