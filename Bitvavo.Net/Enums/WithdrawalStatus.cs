using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Withdrawal status
    /// </summary>
    [JsonConverter(typeof(EnumConverter<WithdrawalStatus>))]
    public enum WithdrawalStatus
    {
        /// <summary>
        /// ["<c>awaiting_processing</c>"] Awaiting processing
        /// </summary>
        [Map("awaiting_processing")]
        AwaitingProcessing,
        /// <summary>
        /// ["<c>awaiting_email_confirmation</c>"] Awaiting email confirmation
        /// </summary>
        [Map("awaiting_email_confirmation")]
        AwaitingEmailConfirmation,
        /// <summary>
        /// ["<c>awaiting_bitvavo_inspection</c>"] Awaiting inspection by Bitvavo
        /// </summary>
        [Map("awaiting_bitvavo_inspection")]
        AwaitingBitvavoInspection,
        /// <summary>
        /// ["<c>approved</c>"] Withdrawal approved
        /// </summary>
        [Map("approved")]
        Approved,
        /// <summary>
        /// ["<c>sending</c>"] Withdrawal is being sent
        /// </summary>
        [Map("sending")]
        Sending,
        /// <summary>
        /// ["<c>in_mempool</c>"] Transaction is in the mempool
        /// </summary>
        [Map("in_mempool")]
        InMempool,
        /// <summary>
        /// ["<c>processed</c>"] Withdrawal processed
        /// </summary>
        [Map("processed")]
        Processed,
        /// <summary>
        /// ["<c>completed</c>"] Transfer completed
        /// </summary>
        [Map("completed")]
        Completed,
        /// <summary>
        /// ["<c>canceled</c>"] Transfer canceled
        /// </summary>
        [Map("canceled")]
        Canceled,
    }
}
