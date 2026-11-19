using CryptoExchange.Net;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Interfaces.Clients;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Threading;
using Bitvavo.Net;
using Bitvavo.Net.Clients;
using Bitvavo.Net.Interfaces;
using Bitvavo.Net.Interfaces.Clients;
using Bitvavo.Net.Objects.Options;
using Bitvavo.Net.SymbolOrderBooks;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Extensions for DI
    /// </summary>
    public static class ServiceCollectionExtensions
    {

        /// <summary>
        /// Add services such as the IBitvavoRestClient and IBitvavoSocketClient. Configures the services based on the provided configuration.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="configuration">The configuration(section) containing the options</param>
        /// <returns></returns>
        public static IServiceCollection AddBitvavo(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = BitvavoOptions.CreateFromConfiguration(configuration);

            services.AddSingleton(Options.Options.Create(options.Rest));
            services.AddSingleton(Options.Options.Create(options.Socket));
            services.AddSingleton(Options.Options.Create(options));

            return AddBitvavoCore(services, options.SocketClientLifeTime);
        }

        /// <summary>
        /// Add services such as the IBitvavoRestClient and IBitvavoSocketClient. Services will be configured based on the provided options.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="optionsDelegate">Set options for the Bitvavo services</param>
        /// <returns></returns>
        public static IServiceCollection AddBitvavo(
            this IServiceCollection services,
            Action<BitvavoOptions>? optionsDelegate = null)
        {
            var options = BitvavoOptions.Create(optionsDelegate);

            services.AddSingleton(Options.Options.Create(options.Rest));
            services.AddSingleton(Options.Options.Create(options.Socket));
            services.AddSingleton(Options.Options.Create(options));

            return AddBitvavoCore(services, options.SocketClientLifeTime);
        }

        private static IServiceCollection AddBitvavoCore(
            this IServiceCollection services,
            ServiceLifetime? socketClientLifeTime = null)
        {
            services.AddHttpClient<IBitvavoRestClient, BitvavoRestClient>((client, serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<BitvavoRestOptions>>().Value;
                client.Timeout = options.RequestTimeout;
                return new BitvavoRestClient(client, serviceProvider.GetRequiredService<ILoggerFactory>(), serviceProvider.GetRequiredService<IOptions<BitvavoRestOptions>>());
            }).ConfigurePrimaryHttpMessageHandler((serviceProvider) => {
                var options = serviceProvider.GetRequiredService<IOptions<BitvavoRestOptions>>().Value;
                return LibraryHelpers.CreateHttpClientMessageHandler(options);
            }).SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            services.Add(new ServiceDescriptor(typeof(IBitvavoSocketClient), x => { return new BitvavoSocketClient(x.GetRequiredService<IOptions<BitvavoSocketOptions>>(), x.GetRequiredService<ILoggerFactory>()); }, socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddTransient<IBitvavoOrderBookFactory, BitvavoOrderBookFactory>();
            services.AddTransient<ITrackerFactory, BitvavoTrackerFactory>();
            services.AddTransient<IBitvavoTrackerFactory, BitvavoTrackerFactory>();
            services.AddSingleton<IBitvavoUserClientProvider, BitvavoUserClientProvider>(x =>
                new BitvavoUserClientProvider(
                    x.GetRequiredService<IHttpClientFactory>().CreateClient(typeof(IBitvavoRestClient).Name),
                    x.GetRequiredService<ILoggerFactory>(),
                    x.GetRequiredService<IOptions<BitvavoRestOptions>>(),
                    x.GetRequiredService<IOptions<BitvavoSocketOptions>>()));

            services.RegisterSharedApiClient<
                IBitvavoSharedApiClient,
                BitvavoSharedApiClient>(sharedApis => sharedApis
                    .Add(client => client.ExchangeRest)
                    .Add(client => client.ExchangeSocket)
                    );

            return services;
        }
    }
}
