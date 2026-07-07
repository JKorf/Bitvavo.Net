using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Withdraw Status.
    /// </summary>
    [JsonConverter(typeof(EnumConverter<WithdrawStatus>))]
    public enum WithdrawStatus
    {
        /// <summary>
        /// ["<c>completed</c>"] Completed.
        /// </summary>
        [Map("completed")]
        Completed,
        /// <summary>
        /// ["<c>awaiting_processing</c>"] Awaiting Processing.
        /// </summary>
        [Map("awaiting_processing")]
        AwaitingProcessing,
        /// <summary>
        /// ["<c>awaiting_email_confirmation</c>"] Awaiting Email Confirmation.
        /// </summary>
        [Map("awaiting_email_confirmation")]
        AwaitingEmailConfirmation,
        /// <summary>
        /// ["<c>canceled</c>"] Canceled.
        /// </summary>
        [Map("canceled")]
        Canceled,
        /// <summary>
        /// ["<c>in_mempool</c>"] In Mempool.
        /// </summary>
        [Map("in_mempool")]
        InMempool,
        /// <summary>
        /// ["<c>awaiting_bitvavo_inspection</c>"] Awaiting Bitvavo Inspection.
        /// </summary>
        [Map("awaiting_bitvavo_inspection")]
        AwaitingBitvavoInspection,
        /// <summary>
        /// ["<c>processing</c>"] Processing.
        /// </summary>
        [Map("processing")]
        Processing,
        /// <summary>
        /// ["<c>rejected</c>"] Rejected.
        /// </summary>
        [Map("rejected")]
        Rejected
    }
}
