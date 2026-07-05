using CryptoExchange.Net.Objects.Options;

namespace Bitvavo.Net.Objects.Options;

/// <summary>
/// Options for the <see cref="Bitvavo.Net.Clients.BitvavoRestClient"/>. 
/// </summary>
public class BitvavoRestOptions : RestExchangeOptions<BitvavoEnvironment, BitvavoCredentials>
{
    /// <summary>
    /// Default options applied to every new <see cref="Bitvavo.Net.Clients.BitvavoRestClient"/>.
    /// </summary>
    internal static BitvavoRestOptions Default { get; set; } = new()
    {
        Environment = BitvavoEnvironment.Live,
    };

    /// <summary>
    /// Construct with defaults applied (Bitvavo Live environment).
    /// </summary>
    public BitvavoRestOptions()
    {
        Default?.Set(this);
    }

    /// <summary>
    /// Spot-API specific configuration.
    /// </summary>
    public RestApiOptions SpotOptions { get; private set; } = new RestApiOptions();

    /// <summary>
    /// Receive window in milliseconds for signed requests. Default is 10 seconds.
    /// </summary>
    public int ReceiveWindowMs { get; set; } = 10_000;

    internal BitvavoRestOptions Set(BitvavoRestOptions targetOptions)
    {
        targetOptions = base.Set(targetOptions);
        targetOptions.SpotOptions = SpotOptions.Set(targetOptions.SpotOptions);
        targetOptions.ReceiveWindowMs = ReceiveWindowMs;
        return targetOptions;
    }
}
