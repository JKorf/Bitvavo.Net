using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Cancellation result for one order
    /// </summary>
    public record BitvavoCanceledOrder
    {
        /// <summary>
        /// ["<c>orderId</c>"] Bitvavo order ID
        /// </summary>
        [JsonPropertyName("orderId")]
        public string Id { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>clientOrderId</c>"] Client order ID when returned
        /// </summary>
        [JsonPropertyName("clientOrderId")]
        public string? ClientOrderId { get; set; }
        /// <summary>
        /// ["<c>operatorId</c>"] Identifier of the trader or bot that requested cancellation
        /// </summary>
        [JsonPropertyName("operatorId")]
        public long? OperatorId { get; set; }
    }
}
