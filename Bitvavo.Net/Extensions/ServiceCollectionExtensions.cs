using Bitvavo.Net;
using Bitvavo.Net.Clients;
using Bitvavo.Net.Interfaces.Clients;
using Bitvavo.Net.Objects.Options;
using CryptoExchange.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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
        public static IServiceCollection AddBitvavo(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            var options = new BitvavoOptions();
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;

            try
            {
                configuration.Bind(options);
            }
            catch (InvalidOperationException ex)
            {
                throw new InvalidOperationException("Invalid configuration provided", ex);
            }

            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            var restEnvName = options.Rest.Environment?.Name ?? options.Environment?.Name ?? BitvavoEnvironment.Live.Name;
            var socketEnvName = options.Socket.Environment?.Name ?? options.Environment?.Name ?? BitvavoEnvironment.Live.Name;
            options.Rest.Environment = BitvavoEnvironment.GetEnvironmentByName(restEnvName) ?? options.Rest.Environment!;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = BitvavoEnvironment.GetEnvironmentByName(socketEnvName) ?? options.Socket.Environment!;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;

            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return services.AddBitvavoCore(options.SocketClientLifeTime);
        }

        /// <summary>
        /// Add services such as the IBitvavoRestClient and IBitvavoSocketClient. Services will be configured based on the provided options.
        /// </summary>
        public static IServiceCollection AddBitvavo(
            this IServiceCollection services,
            Action<BitvavoOptions>? optionsDelegate = null)
        {
            var options = new BitvavoOptions();
            options.Rest.Environment = null!;
            options.Socket.Environment = null!;
            optionsDelegate?.Invoke(options);
            if (options.Rest == null || options.Socket == null)
                throw new ArgumentException("Options null");

            options.Rest.Environment = options.Rest.Environment ?? options.Environment ?? BitvavoEnvironment.Live;
            options.Rest.ApiCredentials = options.Rest.ApiCredentials ?? options.ApiCredentials;
            options.Socket.Environment = options.Socket.Environment ?? options.Environment ?? BitvavoEnvironment.Live;
            options.Socket.ApiCredentials = options.Socket.ApiCredentials ?? options.ApiCredentials;

            services.AddSingleton(x => Options.Options.Create(options.Rest));
            services.AddSingleton(x => Options.Options.Create(options.Socket));

            return services.AddBitvavoCore(options.SocketClientLifeTime);
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
            }).ConfigurePrimaryHttpMessageHandler((serviceProvider) =>
            {
                var options = serviceProvider.GetRequiredService<IOptions<BitvavoRestOptions>>().Value;
                return LibraryHelpers.CreateHttpClientMessageHandler(options);
            }).SetHandlerLifetime(Timeout.InfiniteTimeSpan);
            services.Add(new ServiceDescriptor(typeof(IBitvavoSocketClient), x => new BitvavoSocketClient(x.GetRequiredService<ILoggerFactory>(), x.GetRequiredService<IOptions<BitvavoSocketOptions>>()), socketClientLifeTime ?? ServiceLifetime.Singleton));

            services.AddTransient(x => x.GetRequiredService<IBitvavoRestClient>().SpotApi);
            services.AddTransient(x => x.GetRequiredService<IBitvavoRestClient>().SpotApi.SharedClient);
            services.AddSingleton(x => x.GetRequiredService<IBitvavoSocketClient>().SpotApi);
            services.AddSingleton(x => x.GetRequiredService<IBitvavoSocketClient>().SpotApi.SharedClient);
            services.AddSingleton<IBitvavoUserClientProvider, BitvavoUserClientProvider>(x =>
                new BitvavoUserClientProvider(
                    x.GetRequiredService<IHttpClientFactory>().CreateClient(typeof(IBitvavoRestClient).Name),
                    x.GetRequiredService<ILoggerFactory>(),
                    x.GetRequiredService<IOptions<BitvavoRestOptions>>(),
                    x.GetRequiredService<IOptions<BitvavoSocketOptions>>()));

            services.RegisterSharedRestInterfaces(x => x.GetRequiredService<IBitvavoRestClient>().SpotApi.SharedClient);
            services.RegisterSharedSocketInterfaces(x => x.GetRequiredService<IBitvavoSocketClient>().SpotApi.SharedClient);

            return services;
        }
    }
}
