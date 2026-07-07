using Bitvavo.Net.Objects.Internal;
using CryptoExchange.Net.Objects;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Converters
{
    [JsonSerializable(typeof(Parameters))]
    [JsonSerializable(typeof(BitvavoSocketAuthResponse))]
    [JsonSerializable(typeof(BitvavoSocketRequest))]
    [JsonSerializable(typeof(BitvavoSocketChannel))]
    [JsonSerializable(typeof(BitvavoAllMarketRequest))]
    internal partial class BitvavoSourceGenerationContext : JsonSerializerContext
    {
    }
}
