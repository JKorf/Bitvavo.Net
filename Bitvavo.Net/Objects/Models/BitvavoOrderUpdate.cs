using Bitvavo.Net.Enums;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Order update from the trading channel
    /// </summary>
    public record BitvavoOrderUpdate : BitvavoOrder
    {
        /// <summary>
        /// ["<c>executionType</c>"] Type of order update
        /// </summary>
        [JsonPropertyName("executionType")]
        public OrderExecutionType? ExecutionType { get; set; }
    }
}
