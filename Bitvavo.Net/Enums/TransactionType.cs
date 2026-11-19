using CryptoExchange.Net.Attributes;
using CryptoExchange.Net.Converters.SystemTextJson;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Enums
{
    /// <summary>
    /// Account transaction type
    /// </summary>
    [JsonConverter(typeof(EnumConverter<TransactionType>))]
    public enum TransactionType
    {
        /// <summary>
        /// ["<c>sell</c>"] Sell
        /// </summary>
        [Map("sell")]
        Sell,
        /// <summary>
        /// ["<c>buy</c>"] Buy
        /// </summary>
        [Map("buy")]
        Buy,
        /// <summary>
        /// ["<c>staking</c>"] Staking
        /// </summary>
        [Map("staking")]
        Staking,
        /// <summary>
        /// ["<c>fixed_staking</c>"] Fixed staking
        /// </summary>
        [Map("fixed_staking")]
        FixedStaking,
        /// <summary>
        /// ["<c>deposit</c>"] Deposit
        /// </summary>
        [Map("deposit")]
        Deposit,
        /// <summary>
        /// ["<c>withdrawal</c>"] Withdrawal
        /// </summary>
        [Map("withdrawal")]
        Withdrawal,
        /// <summary>
        /// ["<c>affiliate</c>"] Affiliate reward
        /// </summary>
        [Map("affiliate")]
        Affiliate,
        /// <summary>
        /// ["<c>distribution</c>"] Distribution
        /// </summary>
        [Map("distribution")]
        Distribution,
        /// <summary>
        /// ["<c>internal_transfer</c>"] Internal transfer
        /// </summary>
        [Map("internal_transfer")]
        InternalTransfer,
        /// <summary>
        /// ["<c>withdrawal_cancelled</c>"] Cancelled withdrawal
        /// </summary>
        [Map("withdrawal_cancelled")]
        WithdrawalCancelled,
        /// <summary>
        /// ["<c>rebate</c>"] Rebate
        /// </summary>
        [Map("rebate")]
        Rebate,
        /// <summary>
        /// ["<c>loan</c>"] Loan
        /// </summary>
        [Map("loan")]
        Loan,
        /// <summary>
        /// ["<c>external_transferred_funds</c>"] Externally transferred funds
        /// </summary>
        [Map("external_transferred_funds")]
        ExternalTransferredFunds,
        /// <summary>
        /// ["<c>manually_assigned</c>"] Manually assigned
        /// </summary>
        [Map("manually_assigned")]
        ManuallyAssigned,
        /// <summary>
        /// ["<c>manually_assigned_bitvavo</c>"] Manually assigned by Bitvavo
        /// </summary>
        [Map("manually_assigned_bitvavo")]
        ManuallyAssignedBitvavo,
        /// <summary>
        /// ["<c>margin_loan_collateral_deposit</c>"] Margin loan collateral deposit
        /// </summary>
        [Map("margin_loan_collateral_deposit")]
        MarginLoanCollateralDeposit,
        /// <summary>
        /// ["<c>margin_loan_collateral_return</c>"] Margin loan collateral return
        /// </summary>
        [Map("margin_loan_collateral_return")]
        MarginLoanCollateralReturn,
        /// <summary>
        /// ["<c>margin_loan_borrow</c>"] Margin loan borrowing
        /// </summary>
        [Map("margin_loan_borrow")]
        MarginLoanBorrow,
        /// <summary>
        /// ["<c>margin_loan_repay</c>"] Margin loan repayment
        /// </summary>
        [Map("margin_loan_repay")]
        MarginLoanRepay,
        /// <summary>
        /// ["<c>margin_loan_interest</c>"] Margin loan interest
        /// </summary>
        [Map("margin_loan_interest")]
        MarginLoanInterest,
        /// <summary>
        /// ["<c>margin_loan_liquidation</c>"] Margin loan liquidation
        /// </summary>
        [Map("margin_loan_liquidation")]
        MarginLoanLiquidation,
        /// <summary>
        /// ["<c>convert_buy</c>"] Conversion buy
        /// </summary>
        [Map("convert_buy")]
        ConvertBuy,
        /// <summary>
        /// ["<c>convert_sell</c>"] Conversion sell
        /// </summary>
        [Map("convert_sell")]
        ConvertSell,
        /// <summary>
        /// ["<c>transfer</c>"] Transfer
        /// </summary>
        [Map("transfer")]
        Transfer,
    }
}
