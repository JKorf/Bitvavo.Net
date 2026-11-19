using Bitvavo.Net.Objects.Models;
using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;

namespace Bitvavo.Net.Objects.Internal
{
    internal class BitvavoTicker24hMessage
    {
        [JsonPropertyName("data")]
        [JsonConverter(typeof(BitvavoTicker24hDataConverter))]
        public BitvavoTicker[] Data { get; set; } = [];
    }

    internal class BitvavoTicker24hDataConverter : JsonConverter<BitvavoTicker[]>
    {
        public override BitvavoTicker[] Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.StartObject)
                return [JsonSerializer.Deserialize(ref reader, (JsonTypeInfo<BitvavoTicker>)options.GetTypeInfo(typeof(BitvavoTicker)))!];

            return JsonSerializer.Deserialize(ref reader, (JsonTypeInfo<BitvavoTicker[]>)options.GetTypeInfo(typeof(BitvavoTicker[]))) ?? [];
        }

        public override void Write(Utf8JsonWriter writer, BitvavoTicker[] value, JsonSerializerOptions options)
            => JsonSerializer.Serialize(writer, value, (JsonTypeInfo<BitvavoTicker[]>)options.GetTypeInfo(typeof(BitvavoTicker[])));
    }
}
