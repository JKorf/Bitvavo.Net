using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;
using System.Text.Json;

namespace Bitvavo.Net.Clients.MessageHandlers
{
    internal class BitvavoSocketSpotMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = BitvavoExchange._serializerContext;

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [
            new MessageTypeDefinition
            {
                ForceIfFound = true,
                Fields = [new PropertyFieldReference("requestId")],
                TypeIdentifierCallback = x => x.FieldValue("requestId")!
            }
        ];
    }
}
