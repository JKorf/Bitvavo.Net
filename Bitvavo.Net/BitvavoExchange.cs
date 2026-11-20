using CryptoExchange.Net.Objects;
using CryptoExchange.Net.RateLimiting.Interfaces;
using CryptoExchange.Net.RateLimiting;
using System;
using CryptoExchange.Net.SharedApis;
using Bitvavo.Net.Converters;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters;
using CryptoExchange.Net.RateLimiting.Guards;
using CryptoExchange.Net.RateLimiting.Filters;

namespace Bitvavo.Net
{
    /// <summary>
    /// Bitvavo exchange information and configuration
    /// </summary>
    public static class BitvavoExchange
    {
        internal static JsonSerializerOptions _serializerContext = SerializerOptions.WithConverters(JsonSerializerContextCache.GetOrCreate<BitvavoSourceGenerationContext>());
        internal static ParameterSerializationSettings _parameterSerializationSettings = new ParameterSerializationSettings
        {
            Decimal = DecimalSerialization.String
        };

        /// <summary>
        /// Platform metadata
        /// </summary>
        public static PlatformInfo Metadata { get; } = new PlatformInfo(
                "Bitvavo",
                "Bitvavo",
                "https://raw.githubusercontent.com/JKorf/Bitvavo.Net/main/Bitvavo.Net/Icon/icon.png",
                "https://www.bitvavo.com/",
                ["https://docs.bitvavo.com/docs/get-started/"],
                PlatformType.CryptoCurrencyExchange,
                CentralizationType.Centralized,
                BitvavoEnvironment.All
                );

        /// <summary>
        /// Aliases for Bitvavo assets
        /// </summary>
        public static AssetAliasConfiguration AssetAliases { get; } = new AssetAliasConfiguration
        {
            Aliases = [
                new AssetAlias("USDC", SharedSymbol.UsdOrStable.ToUpperInvariant(), AliasType.OnlyToExchange)
            ]
        };

        /// <summary>
        /// Format a base and quote asset to an Bitvavo recognized symbol 
        /// </summary>
        /// <param name="baseAsset">Base asset</param>
        /// <param name="quoteAsset">Quote asset</param>
        /// <param name="tradingMode">Trading mode</param>
        /// <param name="deliverTime">Delivery time for delivery futures</param>
        /// <returns></returns>
        public static string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverTime = null)
        {
            baseAsset = AssetAliases.CommonToExchangeName(baseAsset.ToUpperInvariant());
            quoteAsset = AssetAliases.CommonToExchangeName(quoteAsset.ToUpperInvariant());

            return $"{baseAsset}-{quoteAsset}";
        }

        /// <summary>
        /// Rate limiter configuration for the Bitvavo API
        /// </summary>
        public static BitvavoRateLimiters RateLimiter { get; } = new BitvavoRateLimiters();
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
        internal BitvavoRateLimiters()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Initialize();
        }

        private void Initialize()
        {
            Bitvavo = new RateLimitGate("Bitvavo")
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerApiKey, new AuthenticatedEndpointFilter(true), 1000, TimeSpan.FromMinutes(1), RateLimitWindowType.Fixed))
                .AddGuard(new RateLimitGuard(RateLimitGuard.PerHost, new AuthenticatedEndpointFilter(false), 1000, TimeSpan.FromMinutes(1), RateLimitWindowType.Fixed));
            Bitvavo.RateLimitTriggered += (x) => RateLimitTriggered?.Invoke(x);
            Bitvavo.RateLimitUpdated += (x) => RateLimitUpdated?.Invoke(x);
        }


        internal IRateLimitGate Bitvavo { get; private set; }

    }
}
