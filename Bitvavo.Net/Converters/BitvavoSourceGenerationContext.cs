using CryptoExchange.Net.Objects;
using System.Text.Json.Serialization;

namespace Bitvavo.Net.Converters
{
    [JsonSerializable(typeof(Parameters))]
    internal partial class BitvavoSourceGenerationContext : JsonSerializerContext
    {
    }
}
