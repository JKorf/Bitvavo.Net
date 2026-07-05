using Bitvavo.Net.Objects.Internal;
using Bitvavo.Net.Objects.Models.Spot.Streams;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using System.Text.Json;

namespace Bitvavo.Net.Clients.MessageHandlers;

/// <summary>
/// Bitvavo spot WebSocket message handler. Extracts the type identifier from the
/// incoming JSON's <c>event</c> field (e.g. <c>candle</c>, <c>trade</c>, <c>subscribed</c>,
/// <c>error</c>), and the per-subscription topic key from the deserialized DTO via
/// <c>AddTopicMapping</c> â€” composite (market+interval) for candles, market for trades.
/// </summary>
internal sealed class BitvavoSocketSpotMessageHandler : JsonSocketMessageHandler
{
    public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(BitvavoExchange._serializerContext);
    public BitvavoSocketSpotMessageHandler()
    {
        AddTopicMapping<BitvavoStreamCandleEvent>(x => x.Market + x.Interval);
        AddTopicMapping<BitvavoStreamTrade>(x => x.Market);
        // Private (account-channel) events: route per-market so two subscriptions on disjoint
        // markets don't see each other's events. The market is always present in the event body.
        AddTopicMapping<BitvavoStreamOrderUpdate>(x => x.Symbol);
        AddTopicMapping<BitvavoStreamFillEvent>(x => x.Symbol);
        AddTopicMapping<BitvavoSocketAuthResponse>(_ => string.Empty);
    }

    protected override MessageTypeDefinition[] TypeEvaluators { get; } = new[]
    {
        new MessageTypeDefinition
        {
            Fields = [new PropertyFieldReference("event")],
            TypeIdentifierCallback = x => x.FieldValue("event")!,
        },
    };
}
