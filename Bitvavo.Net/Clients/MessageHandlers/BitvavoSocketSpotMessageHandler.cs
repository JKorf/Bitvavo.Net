// Copyright (c) Bitvavo.Net contributors. Licensed under the MIT License.

using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson;
using Bitvavo.Net.Objects.Models.Spot.Streams;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using Bitvavo.Net.Objects.Internal;
using Bitvavo.Net.Objects.Models.Spot;

namespace Bitvavo.Net.Clients.MessageHandlers;

internal sealed class BitvavoSocketSpotMessageHandler : JsonSocketMessageHandler
{
    public override JsonSerializerOptions Options { get; } = SerializerOptions.WithConverters(BitvavoExchange._serializerContext);

    public BitvavoSocketSpotMessageHandler()
    {
        AddTopicMapping<BitvavoStreamCandleEvent>(x => x.Symbol + EnumConverter.GetString(x.Interval));
        AddTopicMapping<BitvavoStreamTrade>(x => x.Symbol);
        // Private (account-channel) events: route per-market so two subscriptions on disjoint
        // markets don't see each other's events. The market is always present in the event body.
        AddTopicMapping<BitvavoStreamOrderUpdate>(x => x.Symbol);
        AddTopicMapping<BitvavoStreamFillEvent>(x => x.Symbol);
        AddTopicMapping<BitvavoOrderBookUpdate>(x => x.Symbol);
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
