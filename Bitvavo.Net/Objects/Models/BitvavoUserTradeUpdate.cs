using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// User trade update from the trading channel
    /// </summary>
    public record BitvavoUserTradeUpdate : BitvavoUserTrade
    {
        /// <summary>
        /// ["<c>fillId</c>"] Fill ID
        /// </summary>
        [JsonPropertyName("fillId")]
        public override string Id
        {
            get => base.Id;
            set => base.Id = value;
        }

        /// <summary>
        /// ["<c>timestampNs</c>"] Execution time from the higher precision timestamp
        /// </summary>
        [JsonPropertyName("timestampNs")]
        public DateTime? TimestampPrecise { get; set; }
    }
}
