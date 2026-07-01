using Bitvavo.Net.Converters;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting;
using CryptoExchange.Net.RateLimiting.Filters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.SharedApis;
using System.Text.Json.Serialization;

namespace Bitvavo.Net
{

    /// <summary>
    /// Bitvavo exchange information and configuration
    /// </summary>
    public static class BitvavoExchange
    {
        internal static JsonSerializerContext _serializerContext = JsonSerializerContextCache.GetOrCreate<BitvavoSourceGenerationContext>();
        internal static readonly ParameterSerializationSettings _parameterSerializationSettings = new ParameterSerializationSettings()
        {
            Decimal = DecimalSerialization.String,
        };

        /// <summary>
        /// Platform metadata
        /// </summary>
        public static PlatformInfo Metadata { get; } = new PlatformInfo(
                "Bitvavo",
                "Bitvavo",
                "https://raw.githubusercontent.com/JKorf/Bitvavo.Net/master/Bitvavo.Net/Icon/icon.png",
                "https://bitvavo.com",
                ["https://docs.bitvavo.com"],
                PlatformType.CryptoCurrencyExchange,
                CentralizationType.Centralized,
                BitvavoEnvironment.All
                );

        /// <summary>
        /// Exchange name
        /// </summary>
        public static string ExchangeName => "Bitvavo";

        /// <summary>
        /// Exchange name
        /// </summary>
        public static string DisplayName => "Bitvavo";

        /// <summary>
        /// Url to exchange image
        /// </summary>
        public static string ImageUrl { get; } = "https://raw.githubusercontent.com/JKorf/Bitvavo.Net/master/Bitvavo.Net/Icon/icon.png";

        /// <summary>
        /// Url to the main website
        /// </summary>
        public static string Url { get; } = "https://bitvavo.com";

        /// <summary>
        /// Urls to the API documentation
        /// </summary>
        public static string[] ApiDocsUrl { get; } = new[] {
            "https://docs.bitvavo.com"
            };

        /// <summary>
        /// Type of exchange
        /// </summary>
        public static ExchangeType Type { get; } = ExchangeType.CEX;

        /// <summary>
        /// Aliases for Bitvavo assets
        /// </summary>
        public static AssetAliasConfiguration AssetAliases { get; } = new AssetAliasConfiguration
        {
            Aliases =
            [
                new AssetAlias("USDC", SharedSymbol.UsdOrStable.ToUpperInvariant(), AliasType.OnlyToExchange)
            ]
        };

        /// <summary>
        /// Format a base and quote asset to a Bitvavo recognized symbol 
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns>
        /// </returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            baseAsset = AssetAliases.CommonToExchangeName(baseAsset.ToUpperInvariant());
            quoteAsset = AssetAliases.CommonToExchangeName(quoteAsset.ToUpperInvariant());
            return baseAsset + "-" + quoteAsset;
        }

        /// <summary>
        /// Rate limiter configuration for the Bitvavo API
        /// </summary>
        public static BitvavoRateLimiters RateLimiter { get; set; } = new BitvavoRateLimiters();
    }

    /// <summary>
    /// Rate limiter configuration for the Bitvavo API
    /// </summary>
    public class BitvavoRateLimiters
    {
        /// <summary>
        /// Event for when a rate limit is triggered
        /// </summary>
        public event Action<RateLimitEvent> RateLimitTriggered;
        /// <summary>
        /// Event when the rate limit is updated. Note that it's only updated when a request is send, so there are no specific updates when the current usage is decaying.
        /// </summary>
        public event Action<RateLimitUpdateEvent> RateLimitUpdated;

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        /// <summary>
        /// ctor
        /// </summary>
        public BitvavoRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        /// <summary>
        /// Initialize the rate limits
        /// </summary>
        protected virtual void Initialize()
        {
            Bitvavo = new RateLimitGate("Bitvavo")
                .AddGuard(new RateLimitGuard(
                    keySelector: RateLimitGuard.PerHost,
                    filter: new HostFilter("api.bitvavo.com"),
                    limit: 1000,
                    timeSpan: TimeSpan.FromMinutes(1),
                    windowType: RateLimitWindowType.Sliding));
            Bitvavo.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            Bitvavo.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }

        internal IRateLimitGate Bitvavo { get; private set; }


    }
}
