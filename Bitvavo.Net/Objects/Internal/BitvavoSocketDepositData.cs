using Bitvavo.Net.Objects.Models;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Internal
{
    internal record BitvavoSocketDepositData : BitvavoDepositData
    {
        [JsonPropertyName("paymentId")]
        public string? Memo
        {
            get => PaymentId;
            set => PaymentId = value;
        }
    }
}
