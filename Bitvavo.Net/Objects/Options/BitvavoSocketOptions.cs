using CryptoExchange.Net.Objects.Options;

namespace Bitvavo.Net.Objects.Options
{
    /// <summary>
    /// Options for the BitvavoSocketClient
    /// </summary>
    public class BitvavoSocketOptions : SocketExchangeOptions<BitvavoEnvironment, BitvavoCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static BitvavoSocketOptions Default { get; set; } = new BitvavoSocketOptions()
        {
            Environment = BitvavoEnvironment.Live,
            SocketSubscriptionsCombineTarget = 10
        };


        /// <summary>
        /// ctor
        /// </summary>
        public BitvavoSocketOptions()
        {
            Default?.Set(this);
        }


        /// <summary>
        /// Default operator identifier for trading requests that require one
        /// </summary>
        public long OperatorId { get; set; }

        /// <summary>
        /// Exchange API options
        /// </summary>
        public SocketApiOptions ExchangeOptions { get; private set; } = new SocketApiOptions();


        internal BitvavoSocketOptions Set(BitvavoSocketOptions targetOptions)
        {
            targetOptions = base.Set<BitvavoSocketOptions>(targetOptions);
            targetOptions.ExchangeOptions = ExchangeOptions.Set(targetOptions.ExchangeOptions);
            targetOptions.OperatorId = OperatorId;

            return targetOptions;
        }
    }
}
