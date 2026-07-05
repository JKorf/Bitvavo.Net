using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Withdrawal info
/// </summary>
public record BitvavoWithdrawalHistoryEntry
{
    /// <summary>
    /// Timestamp
    /// </summary>
    [JsonPropertyName("timestamp")]
    public DateTime Timestamp { get; init; }

    /// <summary>
    /// Asset symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Asset { get; init; } = string.Empty;

    /// <summary>
    /// Net amount debited from the account.
    /// </summary>
    [JsonPropertyName("amount")]
    public decimal? Quantity { get; init; }

    /// <summary>
    /// Destination on-chain address
    /// </summary>
    [JsonPropertyName("address")]
    public string? Address { get; init; }

    /// <summary>
    /// Optional payment-reference / memo
    /// </summary>
    [JsonPropertyName("paymentId")]
    public string? PaymentReference { get; init; }

    /// <summary>
    /// On-chain transaction id
    /// </summary>
    [JsonPropertyName("txId")]
    public string? TransactionId { get; init; }

    /// <summary>
    /// Fee
    /// </summary>
    [JsonPropertyName("fee")]
    public decimal? Fee { get; init; }

    /// <summary>
    /// Status
    /// </summary>
    [JsonPropertyName("status")]
    public WithdrawStatus Status { get; init; }
}
