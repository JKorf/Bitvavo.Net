// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using Bitvavo.Net.Enums;

namespace Bitvavo.Net.Objects.Models.Spot;

/// <summary>
/// Asset.
/// </summary>
public record BitvavoAsset
{
    /// <summary>
    /// Symbol.
    /// </summary>
    [JsonPropertyName("symbol")]
    public string Symbol { get; init; } = string.Empty;

    /// <summary>
    /// Name.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; init; } = string.Empty;

    /// <summary>
    /// Decimals.
    /// </summary>
    [JsonPropertyName("decimals")]
    public int Decimals { get; init; }

    /// <summary>
    /// Deposit Fee.
    /// </summary>
    [JsonPropertyName("depositFee")]
    public decimal? DepositFee { get; init; }

    /// <summary>
    /// Deposit Confirmations.
    /// </summary>
    [JsonPropertyName("depositConfirmations")]
    public int DepositConfirmations { get; init; }

    /// <summary>
    /// Deposit Status.
    /// </summary>
    [JsonPropertyName("depositStatus")]
    public AssetStatus DepositStatus { get; init; }

    /// <summary>
    /// Withdrawal Fee.
    /// </summary>
    [JsonPropertyName("withdrawalFee")]
    public decimal? WithdrawalFee { get; init; }

    /// <summary>
    /// Withdrawal Min Quantity.
    /// </summary>
    [JsonPropertyName("withdrawalMinAmount")]
    public decimal? WithdrawalMinQuantity { get; init; }

    /// <summary>
    /// Withdrawal Status.
    /// </summary>
    [JsonPropertyName("withdrawalStatus")]
    public AssetStatus WithdrawalStatus { get; init; }

    /// <summary>
    /// Networks.
    /// </summary>
    [JsonPropertyName("networks")]
    public string[] Networks { get; init; } = System.Array.Empty<string>();

    /// <summary>
    /// Message.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }
}
