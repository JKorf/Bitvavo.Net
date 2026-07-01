using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Bitvavo symbol
/// </summary>
public record BitvavoSymbol
{
    /// <summary>
    /// Symbol identifier
    /// </summary>
    [JsonPropertyName("market")]
    public string Symbol { get; init; } = string.Empty;

    /// <summary>
    /// Trading status
    /// </summary>
    [JsonPropertyName("status")]
    public string Status { get; init; } = string.Empty;

    /// <summary>
    /// Base asset
    /// </summary>
    [JsonPropertyName("base")]
    public string BaseAsset { get; init; } = string.Empty;

    /// <summary>
    /// Quote asset
    /// </summary>
    [JsonPropertyName("quote")]
    public string QuoteAsset { get; init; } = string.Empty;

    /// <summary>
    /// Significant digits used by Bitvavo when displaying prices for this market
    /// </summary>
    [JsonPropertyName("pricePrecision")]
    public int PricePrecision { get; init; }

    /// <summary>
    /// Minimum order size, expressed in the base asset.
    /// </summary>
    [JsonPropertyName("minOrderInBaseAsset")]
    public decimal MinOrderInBaseAsset { get; init; }

    /// <summary>
    /// Minimum order size, expressed in the quote asset.
    /// </summary>
    [JsonPropertyName("minOrderInQuoteAsset")]
    public decimal MinOrderInQuoteAsset { get; init; }

    /// <summary>
    /// Maximum order size, expressed in the base asset.
    /// </summary>
    [JsonPropertyName("maxOrderInBaseAsset")]
    public decimal MaxOrderInBaseAsset { get; init; }

    /// <summary>
    /// Maximum order size, expressed in the quote asset.
    /// </summary>
    [JsonPropertyName("maxOrderInQuoteAsset")]
    public decimal MaxOrderInQuoteAsset { get; init; }

    /// <summary>
    /// Order types supported on this symbol
    /// </summary>
    [JsonPropertyName("orderTypes")]
    public string[] SupportedOrderTypes { get; init; } = [];
}
