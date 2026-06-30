using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Paginated transaction-history response from <c>GET /v2/account/history</c>. Unlike the
/// deposit / withdrawal history endpoints, this endpoint paginates by page number â€” use
/// <see cref="CurrentPage"/> / <see cref="TotalPages"/> to walk the result set.
/// </summary>
public record BitvavoTransactionHistory
{
    /// <summary>
    /// The transactions on the current page.
    /// </summary>
    [JsonPropertyName("items")]
    public IReadOnlyList<BitvavoTransactionHistoryEntry> Items { get; init; } = new List<BitvavoTransactionHistoryEntry>();

    /// <summary>
    /// One-based index of the page returned.
    /// </summary>
    [JsonPropertyName("currentPage")]
    public int CurrentPage { get; init; }

    /// <summary>
    /// Total number of pages available for the requested filter.
    /// </summary>
    [JsonPropertyName("totalPages")]
    public int TotalPages { get; init; }

    /// <summary>
    /// Maximum number of items per page applied to this response.
    /// </summary>
    [JsonPropertyName("maxItems")]
    public int MaxItems { get; init; }
}
