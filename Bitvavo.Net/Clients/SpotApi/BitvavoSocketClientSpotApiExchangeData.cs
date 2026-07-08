using Bitvavo.Net.Enums;
using Bitvavo.Net.Interfaces.Clients.SpotApi;
using Bitvavo.Net.Objects.Internal;
using Bitvavo.Net.Objects.Models.Spot;
using Bitvavo.Net.Objects.Models.Spot.Streams;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Objects;
using CryptoExchange.Net.Objects.Sockets;
using Microsoft.Extensions.Logging;

namespace Bitvavo.Net.Clients.SpotApi;

/// <inheritdoc />
internal sealed class BitvavoSocketClientSpotApiExchangeData : IBitvavoSocketClientSpotApiExchangeData
{
    private readonly ILogger _logger;
    private readonly BitvavoSocketClientSpotApi _client;

    internal BitvavoSocketClientSpotApiExchangeData(ILogger logger, BitvavoSocketClientSpotApi client)
    {
        _logger = logger;
        _client = client;
    }

    /// <inheritdoc />
    public Task<WebSocketResult<UpdateSubscription>> SubscribeToKlineUpdatesAsync(
        string symbol,
        KlineInterval interval,
        Action<DataEvent<BitvavoStreamCandleEvent>> onMessage,
        CancellationToken ct = default)
    {
        var intervalWire = EnumConverter.GetString(interval);
        var subscription = new BitvavoSubscription<BitvavoStreamCandleEvent>(
            _logger,
            typeIdentifier: "candle",
            topic: symbol + intervalWire,
            channel: new BitvavoSocketChannel
            {
                Name = "candles",
                Interval = [intervalWire],
                Symbols = [symbol],
            },
            handler: (receiveTime, originalData, message) =>
                onMessage(new DataEvent<BitvavoStreamCandleEvent>(
                    BitvavoExchange.ExchangeName,
                    message,
                    receiveTime,
                    originalData)
                    .WithSymbol(message.Symbol)
                    .WithStreamId("candles")));

        return _client.SubscribeInternalAsync(_client.BaseAddress, subscription, ct);
    }

    /// <inheritdoc />
    public Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
        string symbol,
        Action<DataEvent<BitvavoStreamTrade>> onMessage,
        CancellationToken ct = default)
    {
        var subscription = new BitvavoSubscription<BitvavoStreamTrade>(
            _logger,
            typeIdentifier: "trade",
            topic: symbol,
            channel: new BitvavoSocketChannel
            {
                Name = "trades",
                Symbols = [symbol],
            },
            handler: (receiveTime, originalData, message) =>
                onMessage(new DataEvent<BitvavoStreamTrade>(
                    BitvavoExchange.ExchangeName,
                    message,
                    receiveTime,
                    originalData)
                    .WithSymbol(message.Symbol)
                    .WithStreamId("trades")));

        return _client.SubscribeInternalAsync(_client.BaseAddress, subscription, ct);
    }

    /// <inheritdoc />
    public Task<WebSocketResult<UpdateSubscription>> SubscribeToTradeUpdatesAsync(
        IEnumerable<string> symbols,
        Action<DataEvent<BitvavoStreamTrade>> onMessage,
        CancellationToken ct = default)
    {
        var marketArray = symbols.ToArray();
        var subscription = new BitvavoSubscription<BitvavoStreamTrade>(
            _logger,
            typeIdentifier: "trade",
            topics: marketArray,
            channel: new BitvavoSocketChannel
            {
                Name = "trades",
                Symbols = marketArray,
            },
            handler: (receiveTime, originalData, message) =>
                onMessage(new DataEvent<BitvavoStreamTrade>(
                    BitvavoExchange.ExchangeName,
                    message,
                    receiveTime,
                    originalData)
                    .WithSymbol(message.Symbol)
                    .WithStreamId("trades")));

        return _client.SubscribeInternalAsync(_client.BaseAddress, subscription, ct);
    }

    /// <inheritdoc />
    public Task<WebSocketResult<UpdateSubscription>> SubscribeToOrderBookUpdatesAsync(
        IEnumerable<string> symbols,
        Action<DataEvent<BitvavoOrderBookUpdate>> onMessage,
        CancellationToken ct = default)
    {
        var marketArray = symbols.ToArray();
        var subscription = new BitvavoSubscription<BitvavoOrderBookUpdate>(
            _logger,
            typeIdentifier: "book",
            topics: marketArray,
            channel: new BitvavoSocketChannel
            {
                Name = "book",
                Symbols = marketArray,
            },
            handler: (receiveTime, originalData, message) =>
                onMessage(new DataEvent<BitvavoOrderBookUpdate>(
                    BitvavoExchange.ExchangeName,
                    message,
                    receiveTime,
                    originalData)
                    .WithSymbol(message.Symbol)
                    .WithStreamId("book")));

        return _client.SubscribeInternalAsync(_client.BaseAddress, subscription, ct);
    }
}
