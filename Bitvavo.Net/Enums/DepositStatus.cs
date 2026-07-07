// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json.Serialization;
using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;

namespace Bitvavo.Net.Enums;

/// <summary>
/// Deposit Status.
/// </summary>
[JsonConverter(typeof(EnumConverter<DepositStatus>))]
public enum DepositStatus
{
    /// <summary>
    /// ["<c>completed</c>"] Completed.
    /// </summary>
    [Map("completed")] Completed = 0,
    /// <summary>
    /// ["<c>awaiting_processing</c>"] Awaiting Processing.
    /// </summary>
    [Map("awaiting_processing")] AwaitingProcessing = 1,
    /// <summary>
    /// ["<c>awaiting_email_confirmation</c>"] Awaiting Email Confirmation.
    /// </summary>
    [Map("awaiting_email_confirmation")] AwaitingEmailConfirmation = 2,
    /// <summary>
    /// ["<c>awaiting_bitvavo_inspection</c>"] Awaiting Bitvavo Inspection.
    /// </summary>
    [Map("awaiting_bitvavo_inspection")] AwaitingBitvavoInspection = 3,
    /// <summary>
    /// ["<c>in_mempool</c>"] In Mempool.
    /// </summary>
    [Map("in_mempool")] InMempool = 4,
    /// <summary>
    /// ["<c>processing</c>"] Processing.
    /// </summary>
    [Map("processing")] Processing = 5,
    /// <summary>
    /// ["<c>rejected</c>"] Rejected.
    /// </summary>
    [Map("rejected")] Rejected = 6,
}
