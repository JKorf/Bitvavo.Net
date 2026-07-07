// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Deposit Address.
/// </summary>
public record BitvavoDepositAddress
{
    /// <summary>
    /// Address.
    /// </summary>
    [JsonPropertyName("address")]
    public string? Address { get; init; }

    /// <summary>
    /// IBAN.
    /// </summary>
    [JsonPropertyName("iban")]
    public string? Iban { get; init; }

    /// <summary>
    /// BIC.
    /// </summary>
    [JsonPropertyName("bic")]
    public string? Bic { get; init; }

    /// <summary>
    /// Payment Reference.
    /// </summary>
    [JsonPropertyName("paymentId")]
    public string? PaymentReference { get; init; }

    /// <summary>
    /// Description.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }
}
