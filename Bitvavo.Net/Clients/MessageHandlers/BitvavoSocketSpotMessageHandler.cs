using CryptoExchange.Net.Converters.MessageParsing.DynamicConverters;
using System.Text.Json;
using CryptoExchange.Net.Converters.SystemTextJson.MessageHandlers;

namespace Bitvavo.Net.Clients.MessageHandlers
{
    internal class BitvavoSocketSpotMessageHandler : JsonSocketMessageHandler
    {
        public override JsonSerializerOptions Options { get; } = BitvavoExchange._serializerContext;

        public BitvavoSocketSpotMessageHandler()
        {
        }

        protected override MessageTypeDefinition[] TypeEvaluators { get; } = [

#warning TODO

            //new MessageTypeDefinition {
            //    Fields = [
            //        new PropertyFieldReference("stream"),
            //    ],
            //    TypeIdentifierCallback = x => x.FieldValue("stream")!,
            //},

            //new MessageTypeDefinition {
            //    ForceIfFound = true,
            //    Fields = [
            //        new PropertyFieldReference("id"),
            //    ],
            //    TypeIdentifierCallback = x => x.FieldValue("id")!,
            //}
        ];
    }
}
