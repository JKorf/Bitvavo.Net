using System;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    internal record BitvavoServerTime
    {
        /// <summary>
        /// ["<c>time</c>"] Server time
        /// </summary>
        [JsonPropertyName("time")]
        public DateTime Time { get; set; }
    }
}
