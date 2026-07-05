using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Withdrawal result
/// </summary>
public record BitvavoWithdrawalResult
{
    /// <summary>
    /// Indicates Bitvavo accepted the withdrawal request
    /// </summary>
    [JsonPropertyName("success")]
    public bool Success { get; init; }

    /// <summary>
    /// Asset that was withdrawn
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Asset { get; init; } = string.Empty;

    /// <summary>
    /// Total amount debited from the account
    /// </summary>
    [JsonPropertyName("amount"), JsonConverter(typeof(DecimalConverter))]
    public decimal? Quantity { get; init; }
}
