// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Transaction Type.
/// </summary>
[JsonConverter(typeof(EnumConverter<TransactionType>))]
public enum TransactionType
{
    /// <summary>
    /// ["<c>sell</c>"] Sell.
    /// </summary>
    [Map("sell")] Sell = 0,
    /// <summary>
    /// ["<c>buy</c>"] Buy.
    /// </summary>
    [Map("buy")] Buy = 1,
    /// <summary>
    /// ["<c>staking</c>"] Staking.
    /// </summary>
    [Map("staking")] Staking = 2,
    /// <summary>
    /// ["<c>fixed_staking</c>"] Fixed Staking.
    /// </summary>
    [Map("fixed_staking")] FixedStaking = 3,
    /// <summary>
    /// ["<c>deposit</c>"] Deposit.
    /// </summary>
    [Map("deposit")] Deposit = 4,
    /// <summary>
    /// ["<c>withdrawal</c>"] Withdrawal.
    /// </summary>
    [Map("withdrawal")] Withdrawal = 5,
    /// <summary>
    /// ["<c>affiliate</c>"] Affiliate.
    /// </summary>
    [Map("affiliate")] Affiliate = 6,
    /// <summary>
    /// ["<c>distribution</c>"] Distribution.
    /// </summary>
    [Map("distribution")] Distribution = 7,
    /// <summary>
    /// ["<c>internal_transfer</c>"] Internal Transfer.
    /// </summary>
    [Map("internal_transfer")] InternalTransfer = 8,
    /// <summary>
    /// ["<c>withdrawal_cancelled</c>"] Withdrawal Cancelled.
    /// </summary>
    [Map("withdrawal_cancelled")] WithdrawalCancelled = 9,
    /// <summary>
    /// ["<c>rebate</c>"] Rebate.
    /// </summary>
    [Map("rebate")] Rebate = 10,
    /// <summary>
    /// ["<c>loan</c>"] Loan.
    /// </summary>
    [Map("loan")] Loan = 11,
    /// <summary>
    /// ["<c>external_transferred_funds</c>"] External Transferred Funds.
    /// </summary>
    [Map("external_transferred_funds")] ExternalTransferredFunds = 12,
    /// <summary>
    /// ["<c>manually_assigned</c>"] Manually Assigned.
    /// </summary>
    [Map("manually_assigned")] ManuallyAssigned = 13,
}
