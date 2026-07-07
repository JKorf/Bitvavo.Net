// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Subaccount Balances.
/// </summary>
public record BitvavoSubaccountBalances
{
    /// <summary>
    /// Balances.
    /// </summary>
    [JsonPropertyName("balances")]
    public BitvavoBalance[] Balances { get; init; } = System.Array.Empty<BitvavoBalance>();
}
