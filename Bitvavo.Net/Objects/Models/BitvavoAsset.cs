using Bitvavo.Net.Enums;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models
{
    /// <summary>
    /// Asset information
    /// </summary>
    public record BitvavoAsset
    {
        /// <summary>
        /// ["<c>symbol</c>"] Asset symbol
        /// </summary>
        [JsonPropertyName("symbol")]
        public string Asset { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>name</c>"] Asset name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;
        /// <summary>
        /// ["<c>decimals</c>"] Number of decimal places for the asset
        /// </summary>
        [JsonPropertyName("decimals")]
        public int Decimals { get; set; }
        /// <summary>
        /// ["<c>depositFee</c>"] Deposit fee in the asset
        /// </summary>
        [JsonPropertyName("depositFee")]
        public decimal DepositFee { get; set; }
        /// <summary>
        /// ["<c>depositConfirmations</c>"] Network confirmations required to credit a deposit
        /// </summary>
        [JsonPropertyName("depositConfirmations")]
        public int DepositConfirmations { get; set; }
        /// <summary>
        /// ["<c>depositStatus</c>"] Deposit status
        /// </summary>
        [JsonPropertyName("depositStatus")]
        public AssetTransferStatus DepositStatus { get; set; }
        /// <summary>
        /// ["<c>withdrawalFee</c>"] Withdrawal fee in the asset
        /// </summary>
        [JsonPropertyName("withdrawalFee")]
        public decimal WithdrawalFee { get; set; }
        /// <summary>
        /// ["<c>withdrawalMinAmount</c>"] Minimum withdrawal quantity
        /// </summary>
        [JsonPropertyName("withdrawalMinAmount")]
        public decimal MinWithdrawalQuantity { get; set; }
        /// <summary>
        /// ["<c>withdrawalStatus</c>"] Withdrawal status
        /// </summary>
        [JsonPropertyName("withdrawalStatus")]
        public AssetTransferStatus WithdrawalStatus { get; set; }
        /// <summary>
        /// ["<c>networks</c>"] Supported networks
        /// </summary>
        [JsonPropertyName("networks")]
        public string[] Networks { get; set; } = [];
        /// <summary>
        /// ["<c>message</c>"] Reason for a deposit or withdrawal restriction
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = string.Empty;
    }
}
