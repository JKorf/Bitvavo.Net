using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Trading fees and 30-day activity for the account
    /// </summary>
    public record BitvavoAccountFees
    {
        /// <summary>
        /// ["<c>maker</c>"] Maker fee rate
        /// </summary>
        [JsonPropertyName("maker")]
        public decimal MakerFee { get; set; }
        /// <summary>
        /// ["<c>taker</c>"] Taker fee rate
        /// </summary>
        [JsonPropertyName("taker")]
        public decimal TakerFee { get; set; }
        /// <summary>
        /// ["<c>volume</c>"] Trading quantity over the last 30 days, expressed in EUR
        /// </summary>
        [JsonPropertyName("volume")]
        public decimal TradingQuantity { get; set; }
    }

    internal record BitvavoAccountFeesResponse
    {
        /// <summary>
        /// ["<c>fees</c>"] Account trading fees
        /// </summary>
        [JsonPropertyName("fees")]
        public BitvavoAccountFees Fees { get; set; } = new();
    }
}
