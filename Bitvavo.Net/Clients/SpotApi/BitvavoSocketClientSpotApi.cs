using System.Net.WebSockets;
using Bitvavo.Net.Clients.MessageHandlers;
using Bitvavo.Net.Interfaces.Clients.SpotApi;
using Bitvavo.Net.Objects.Internal;
using Bitvavo.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.Objects.Sockets;
using CryptoExchange.Net.SharedApis;
using CryptoExchange.Net.Sockets;
using CryptoExchange.Net.Sockets.Default;
using Microsoft.Extensions.Logging;

namespace Bitvavo.Net.Clients.SpotApi;

/// <inheritdoc cref="IBitvavoSocketClientSpotApi" />
internal sealed partial class BitvavoSocketClientSpotApi : SocketApiClient<BitvavoEnvironment, BitvavoAuthenticationProvider, BitvavoCredentials>, IBitvavoSocketClientSpotApi
{
    /// <inheritdoc />
    public new BitvavoSocketOptions ClientOptions => (BitvavoSocketOptions)base.ClientOptions;

    /// <inheritdoc />
    protected override ErrorMapping ErrorMapping => BitvavoErrors.SpotErrors;

    /// <inheritdoc />
    public IBitvavoSocketClientSpotApiExchangeData ExchangeData { get; }

    /// <inheritdoc />
    public IBitvavoSocketClientSpotApiAccount Account { get; }

    /// <inheritdoc />
    public IBitvavoSocketClientSpotApiShared SharedClient => this;

    internal BitvavoSocketClientSpotApi(ILoggerFactory? loggerFactory, BitvavoSocketOptions options)
        : base(loggerFactory, BitvavoExchange.Metadata.Id, options.Environment.SpotSocketPublicAddress, options, options.SpotOptions)
    {
        ExchangeData = new BitvavoSocketClientSpotApiExchangeData(_logger, this);
        Account = new BitvavoSocketClientSpotApiAccount(_logger, this);
    }

    /// <inheritdoc />
    protected override IMessageSerializer CreateSerializer() =>
        new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BitvavoExchange._serializerContext));

    /// <inheritdoc />
    public override ISocketMessageHandler CreateMessageConverter(WebSocketMessageType messageType) =>
        new BitvavoSocketSpotMessageHandler();

    /// <inheritdoc />
    public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
        => BitvavoExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

    /// <inheritdoc />
    protected override BitvavoAuthenticationProvider CreateAuthenticationProvider(BitvavoCredentials credentials)
        => new(credentials, ClientOptions.ReceiveWindowMs);

    /// <inheritdoc />
    protected override Task<Query?> GetAuthenticationRequestAsync(SocketConnection connection)
    {
        var provider = AuthenticationProvider!;
        var payload = provider.BuildSocketAuth();
        return Task.FromResult<Query?>(new BitvavoSocketAuthQuery(payload));
    }

    /// <summary>
    /// Internal wrapper for the inherited protected
    /// <c>SocketApiClient.SubscribeAsync(string, Subscription, CancellationToken)</c> so peer
    /// classes (the *Data partials) can subscribe without being derived from this class.
    /// </summary>
    internal Task<WebSocketResult<UpdateSubscription>> SubscribeInternalAsync(
        string url,
        Subscription subscription,
        CancellationToken ct)
        => SubscribeAsync(url, subscription, ct);
}
