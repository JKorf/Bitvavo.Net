using Bitvavo.Net.Enums;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Symbol.
/// </summary>
public record BitvavoSymbol
{
    /// <summary>
    /// Symbol.
    /// </summary>
    [JsonPropertyName("market")]
    public string Symbol { get; init; } = string.Empty;

    /// <summary>
    /// Status.
    /// </summary>
    [JsonPropertyName("status")]
    public SymbolStatus Status { get; init; }

    /// <summary>
    /// Base Asset.
    /// </summary>
    [JsonPropertyName("base")]
    public string BaseAsset { get; init; } = string.Empty;

    /// <summary>
    /// Quote Asset.
    /// </summary>
    [JsonPropertyName("quote")]
    public string QuoteAsset { get; init; } = string.Empty;

    /// <summary>
    /// Price Precision.
    /// </summary>
    [JsonPropertyName("pricePrecision")]
    public int PricePrecision { get; init; }

    /// <summary>
    /// Min Order In Base Asset.
    /// </summary>
    [JsonPropertyName("minOrderInBaseAsset")]
    public decimal MinOrderInBaseAsset { get; init; }

    /// <summary>
    /// Min Order In Quote Asset.
    /// </summary>
    [JsonPropertyName("minOrderInQuoteAsset")]
    public decimal MinOrderInQuoteAsset { get; init; }

    /// <summary>
    /// Max Order In Base Asset.
    /// </summary>
    [JsonPropertyName("maxOrderInBaseAsset")]
    public decimal MaxOrderInBaseAsset { get; init; }

    /// <summary>
    /// Max Order In Quote Asset.
    /// </summary>
    [JsonPropertyName("maxOrderInQuoteAsset")]
    public decimal MaxOrderInQuoteAsset { get; init; }

    /// <summary>
    /// Supported Order Types.
    /// </summary>
    [JsonPropertyName("orderTypes")]
    public string[] SupportedOrderTypes { get; init; } = [];
}
