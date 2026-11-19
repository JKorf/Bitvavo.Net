using Bitvavo.Net.Objects.Internal;
using Bitvavo.Net.Objects.Models;
using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using System.Text.Json;

namespace Bitvavo.Net.Clients.MessageHandlers
{
    internal class BitvavoSocketSpotMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = BitvavoExchange._serializerContext;

        public BitvavoSocketSpotMessageHandler()
        {
            AddTopicMapping<BitvavoTickerUpdate>(update => update.Symbol);
            AddTopicMapping<BitvavoTradeUpdate>(update => update.Symbol);
            AddTopicMapping<BitvavoOrderBook>(update => update.Symbol);
            AddTopicMapping<BitvavoKlineUpdate>(update => BitvavoSocketChannel.GetTopicFilter(update.Symbol, update.Interval));
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [
            new MessageTypeDefinition
            {
                ForceIfFound = true,
                Fields = [new PropertyFieldReference("requestId")],
                TypeIdentifierCallback = x => x.FieldValue("requestId")!
            },
            new MessageTypeDefinition
            {
                Fields = [new PropertyFieldReference("event")],
                TypeIdentifierCallback = x =>
                {
                    var eventName = x.FieldValue("event");
                    return eventName == "candles" ? "candle" : eventName!;
                }
            }
        ];
    }
}
