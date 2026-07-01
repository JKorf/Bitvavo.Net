using System.Text.Json.Serialization;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Balance entry for a single asset
/// </summary>
public record BitvavoBalance
{
    /// <summary>
    /// Asset symbol
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Asset { get; init; } = string.Empty;

    /// <summary>
    /// Amount that's free to trade or withdraw.
    /// </summary>
    [JsonPropertyName("available"), JsonConverter(typeof(DecimalConverter))]
    public decimal? Available { get; init; }

    /// <summary>
    /// Amount reserved by open orders or pending withdrawals.
    /// </summary>
    [JsonPropertyName("inOrder"), JsonConverter(typeof(DecimalConverter))]
    public decimal? InOrder { get; init; }
}
