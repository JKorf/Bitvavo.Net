using Bitvavo.Net.Clients.MessageHandlers;
using Bitvavo.Net.Interfaces.Clients.SpotApi;
using Bitvavo.Net.Objects.Options;
using CryptoExchange.Net.Clients;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Interfaces;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Errors;
using CryptoExchange.Net.SharedApis;
using Microsoft.Extensions.Logging;

namespace Bitvavo.Net.Clients.SpotApi;

/// <inheritdoc cref="IBitvavoRestClientSpotApi" />
internal partial class BitvavoRestClientSpotApi : RestApiClient<BitvavoEnvironment, BitvavoAuthenticationProvider, BitvavoCredentials>, IBitvavoRestClientSpotApi
{
    /// <inheritdoc />
    public new BitvavoRestOptions ClientOptions => (BitvavoRestOptions)base.ClientOptions;

    /// <inheritdoc />
    protected override ErrorMapping ErrorMapping => BitvavoErrors.SpotErrors;

    /// <inheritdoc />
    protected override IRestMessageHandler MessageHandler { get; } = new BitvavoRestSpotMessageHandler(BitvavoErrors.SpotErrors);


    /// <inheritdoc />
    public IBitvavoRestClientSpotApiAccount Account { get; }

    /// <inheritdoc />
    public IBitvavoRestClientSpotApiExchangeData ExchangeData { get; }
    
    /// <inheritdoc />
    public IBitvavoRestClientSpotApiTrading Trading { get; }

    /// <inheritdoc />
    public IBitvavoRestClientSpotApiInstitutional Institutional { get; }

    /// <inheritdoc />
    public IBitvavoRestClientSpotApiShared SharedClient => this;

    internal BitvavoRestClientSpotApi(ILoggerFactory? loggerFactory, HttpClient? httpClient, BitvavoRestOptions options)
        : base(loggerFactory, BitvavoExchange.Metadata.Id, httpClient, options.Environment.SpotRestBaseAddress, options, options.SpotOptions)
    {
        ExchangeData = new BitvavoRestClientSpotApiExchangeData(this);
        Account = new BitvavoRestClientSpotApiAccount(this);
        Trading = new BitvavoRestClientSpotApiTrading(this);
        Institutional = new BitvavoRestClientSpotApiInstitutional(this);

        ParameterPositions = new Dictionary<HttpMethod, HttpMethodParameterPosition>
        {
            { HttpMethod.Delete, HttpMethodParameterPosition.InUri }
        };
    }

    /// <inheritdoc />
    public override string FormatSymbol(string baseAsset, string quoteAsset, TradingMode tradingMode, DateTime? deliverDate = null)
        => BitvavoExchange.FormatSymbol(baseAsset, quoteAsset, tradingMode, deliverDate);

    /// <inheritdoc />
    protected override BitvavoAuthenticationProvider CreateAuthenticationProvider(BitvavoCredentials credentials)
        => new(credentials, ClientOptions.ReceiveWindowMs);

    /// <inheritdoc />
    protected override IMessageSerializer CreateSerializer() 
        => new SystemTextJsonMessageSerializer(SerializerOptions.WithConverters(BitvavoExchange._serializerContext));

    internal Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? parameters, CancellationToken cancellationToken, int? weight = null)
        => SendAsync<T>(definition, parameters, cancellationToken, weight: weight);

    internal Task<HttpResult<T>> SendAsync<T>(RequestDefinition definition, Parameters? queryParameters, Parameters? bodyParameters, CancellationToken cancellationToken, int? weight = null)
        => SendAsync<T>(definition, queryParameters, bodyParameters, cancellationToken, weight: weight);
}
