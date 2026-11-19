using CryptoExchange.Net.Objects.Options;

namespace Bitvavo.Net.Objects.Options
{
    /// <summary>
    /// Options for the BitvavoRestClient
    /// </summary>
    public class BitvavoRestOptions : RestExchangeOptions<BitvavoEnvironment, BitvavoCredentials>
    {
        /// <summary>
        /// Default options for new clients
        /// </summary>
        internal static BitvavoRestOptions Default { get; set; } = new BitvavoRestOptions()
        {
            Environment = BitvavoEnvironment.Live,
            AutoTimestamp = true
        };

        /// <summary>
        /// ctor
        /// </summary>
        public BitvavoRestOptions()
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
        public RestApiOptions ExchangeOptions { get; private set; } = new RestApiOptions();


        internal BitvavoRestOptions Set(BitvavoRestOptions targetOptions)
        {
            targetOptions = base.Set<BitvavoRestOptions>(targetOptions);
            targetOptions.ExchangeOptions = ExchangeOptions.Set(targetOptions.ExchangeOptions);
            targetOptions.OperatorId = OperatorId;

            return targetOptions;
        }
    }
}
