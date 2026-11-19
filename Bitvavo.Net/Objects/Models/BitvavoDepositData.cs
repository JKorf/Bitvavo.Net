using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Deposit instructions for a fiat or crypto asset
    /// </summary>
    public record BitvavoDepositData
    {
        /// <summary>
        /// ["<c>address</c>"] Crypto deposit address
        /// </summary>
        [JsonPropertyName("address")]
        public string? Address { get; set; }
        /// <summary>
        /// ["<c>paymentid</c>"] Memo or tag to include with a crypto deposit
        /// </summary>
        [JsonPropertyName("paymentid")]
        public string? PaymentId { get; set; }
        /// <summary>
        /// ["<c>iban</c>"] Bank account number for a fiat deposit
        /// </summary>
        [JsonPropertyName("iban")]
        public string? Iban { get; set; }
        /// <summary>
        /// ["<c>bic</c>"] Bank identifier code for a fiat deposit
        /// </summary>
        [JsonPropertyName("bic")]
        public string? Bic { get; set; }
        /// <summary>
        /// ["<c>description</c>"] Payment reference to include with a fiat deposit
        /// </summary>
        [JsonPropertyName("description")]
        public string? Description { get; set; }
    }
}
